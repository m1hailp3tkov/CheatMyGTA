using CheatMyGTA.Contracts;
using CheatMyGTA.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheatMyGTA
{
    /// <summary>
    /// Interaction logic for KeyBindsWindow.xaml
    /// </summary>
    public partial class KeyBindsWindow : Window
    {
        private readonly IKeyBinds keyBinds;
        private readonly IGame game;
        private IDictionary<Key, string> keyBindsDictionary;
        private bool edited;

        public KeyBindsWindow(IKeyBinds keyBinds, IGame game)
        {
            InitializeComponent();

            this.edited = false;
            this.keyBinds = keyBinds;
            this.game = game;
            this.keyBindsDictionary = this.keyBinds.GetKeyBinds(game).ToDictionary(x => x.Key, y => y.Value);

            CreateChildren(game.CheatCodes);

            this.Closing += KeyBindsWindow_Closing;
        }

        private void KeyBindsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(edited)
            {
                var result = MessageBox.Show("Save changes?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    //TODO: Save logic
                }
                else if(result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void CreateChildren(IReadOnlyDictionary<string, string> cheatCodes)
        {
            foreach(var cheat in game.CheatCodes)
            {
                var cheatCodeKey = new CheatCodeKey(cheat);

                var boundKey = keyBindsDictionary.FirstOrDefault(x => x.Value == cheat.Key);

                if (!boundKey.Equals(default(KeyValuePair<Key, string>)))
                {
                    cheatCodeKey.KeyButton.Content = boundKey.Key.ToString();
                }

                cheatCodeKey.KeyChanged += CheatCodeKey_KeyChanged;
                cheatCodeKey.KeyButton.Click += KeyButton_Click;

                this.StackPanel.Children.Add(cheatCodeKey);
            }
        }

        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            this.HelpLabel.Content = "Press the new key (ESC to cancel)";

            foreach(var cheatCodeKey in this.StackPanel.Children.OfType<CheatCodeKey>())
            {
                if(cheatCodeKey.KeyButton != sender)
                {
                    cheatCodeKey.KeyButton.Background = Brushes.LightGray;
                }
            }
        }

        private void CheatCodeKey_KeyChanged(object sender, KeyPressEventArgs e)
        {
            this.HelpLabel.Content = "";
            this.edited = this.edited || e.Edited;

            if (e.Key == null) return;

            var btn = this.StackPanel.Children
                .OfType<CheatCodeKey>()
                .FirstOrDefault(x => x.Key == e.Key);

            if (btn == null) return;

            if(btn != sender)
            {
                btn.Key = null;
            }
        }
    }
}
