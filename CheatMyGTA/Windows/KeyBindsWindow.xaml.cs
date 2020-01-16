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

        public KeyBindsWindow(IKeyBinds keyBinds, IGame game)
        {
            InitializeComponent();
            this.keyBinds = keyBinds;
            this.game = game;
            this.keyBindsDictionary = this.keyBinds.GetKeyBinds(game).ToDictionary(x => x.Key, y => y.Value);

            CreateChildren(game.CheatCodes);
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
            foreach(var cheatCodeKey in this.StackPanel.Children.OfType<CheatCodeKey>())
            {
                if(cheatCodeKey.KeyButton != sender)
                {
                    cheatCodeKey.KeyButton.Background = Brushes.LightGray;
                }
            }
        }

        private void CheatCodeKey_KeyChanged(object sender, KeyChangedEventArgs e)
        {
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
