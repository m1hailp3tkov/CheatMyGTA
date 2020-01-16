using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CheatMyGTA.UserControls
{
    /// <summary>
    /// Interaction logic for CheatCodeKey.xaml
    /// </summary>
    public partial class CheatCodeKey : UserControl
    {
        private Nullable<Key> key;

        public CheatCodeKey(KeyValuePair<string, string> cheat)
        {
            this.CheatCode = cheat.Value;

            InitializeComponent();

            this.Label.Content = this.CheatCode;
            this.Label.ToolTip = cheat.Key;
        }

        public Nullable<Key> Key 
        {
            get => this.key;
            set
            {
                if (value != null)
                {
                    this.KeyChanged(this, new KeyChangedEventArgs { Key = value });
                }

                this.key = value;
                this.KeyButton.Content = value.ToString();
            }
        }
        public string CheatCode { get; private set; }

        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Background = Brushes.Red;

            ((Button)sender).KeyDown += SenderBtn_KeyDown;
        }

        private void SenderBtn_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key != System.Windows.Input.Key.Escape)
            {
                this.Key = e.Key;
            }

            ((Button)sender).Background = Brushes.LightGray;

            ((Button)sender).KeyDown -= SenderBtn_KeyDown;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public event EventHandler<KeyChangedEventArgs> KeyChanged;

    }

    public class KeyChangedEventArgs : EventArgs
    {
        public Nullable<Key> Key { get; set; }
    }
}
