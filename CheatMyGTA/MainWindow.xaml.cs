using Ownskit.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using CheatMyGTA.Helpers;
using CheatMyGTA.Contracts;
using CheatMyGTA.Models;

namespace CheatMyGTA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyboardListener keyboardListener;

        public MainWindow(IGameSource gameSource)
        {
            InitializeComponent();

            gameSource.Load();
            gamesComboBox.ItemsSource = gameSource.GameList;

            this.keyboardListener = new KeyboardListener();
        }

        private void AttachToGame(object sender, RoutedEventArgs e)
        {
            if(gamesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a game.");
                return;
            }

            var game = (IGame)gamesComboBox.SelectedItem;
            var process = Process.GetProcessesByName(game.ProcessName)
                .FirstOrDefault();

            if(process == null)
            {
                MessageBox.Show($"Process {game.Process} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var msgBoxResult = MessageBox.Show("Process found. Go to game?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if(msgBoxResult == MessageBoxResult.Yes)
            {
                Win32Methods.BringToFront(process);
            }
        }
    }
}
