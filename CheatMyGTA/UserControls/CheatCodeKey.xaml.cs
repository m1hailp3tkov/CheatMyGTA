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
            bool edited = false;

            if (e.Key != System.Windows.Input.Key.Escape && e.Key != this.Key)
            {
                edited = true;
                this.Key = e.Key;
            }

            this.KeyChanged(this, new KeyPressEventArgs { Key = e.Key, Edited = edited });

            ((Button)sender).Background = Brushes.LightGray;

            ((Button)sender).KeyDown -= SenderBtn_KeyDown;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            bool edited = this.Key == null;
            this.Key = null;
            this.KeyChanged(this, new KeyPressEventArgs { Key = null, Edited = edited });
        }

        public event EventHandler<KeyPressEventArgs> KeyChanged;

    }

    public class KeyPressEventArgs : EventArgs
    {
        public Nullable<Key> Key { get; set; }
        public bool Edited { get; set; }
    }
}
