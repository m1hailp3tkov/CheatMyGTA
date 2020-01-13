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
using System.Windows.Forms;

namespace CheatMyGTA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICheatBinder cheatBinder;

        private KeyboardListener keyboardListener;
        private NativeMethods nativeMethods;
        private Process process;

        private bool enabled;
        private bool isFocused;

        public MainWindow(IGameSource gameSource, ICheatBinder cheatBinder)
        {
            InitializeComponent();

            SetStatus(false);
            isFocused = false;

            gameSource.Load();
            gamesComboBox.ItemsSource = gameSource.GameList;

            this.nativeMethods = new NativeMethods();
            this.keyboardListener = new KeyboardListener();
            keyboardListener.KeyDown += OnKeyPress;
            
            this.cheatBinder = cheatBinder;

            nativeMethods.WindowChanged += NativeMethods_WindowChanged;
        }

        private void NativeMethods_WindowChanged(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (process == null) return;

            if (NativeMethods.GetProcessIdFromHandle(hwnd) == process.Id)
            {
                isFocused = true;
            }
            else
            {
                isFocused = false;
            }
        }

        private void OnKeyPress(object sender, RawKeyEventArgs args)
        {
            if (enabled && isFocused)
            {
                var cheatCode = cheatBinder.GetCheatCode(args.Key);

                if (!string.IsNullOrEmpty(cheatCode))
                {
                    SendKeys.SendWait(cheatCode);
                }
            }
        }

        private void AttachToGame(object sender, RoutedEventArgs e)
        {
            var game = (IGame)gamesComboBox.SelectedItem;

            if (game == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a game.");
                return;
            }

            process = Process.GetProcessesByName(game.ProcessName)
                .FirstOrDefault();

            if (process == null)
            {
                System.Windows.MessageBox.Show($"Process {game.Process} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            cheatBinder.ActiveGame = game;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            SetStatus(true, $"{game.Process}");

            var msgBoxResult = System.Windows.MessageBox.Show("Process found. Go to game?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (msgBoxResult == System.Windows.MessageBoxResult.Yes)
            {
                NativeMethods.BringToFront(process);
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if(enabled)
            {
                this.Dispatcher.Invoke(() =>
                {
                    SetStatus(false, "Game exited");
                });
            }
        }

        private void SetStatus(bool enabled, string labelContent = null)
        {
            this.enabled = enabled;
            if (!enabled)
            {
                process = null;
            }

            //UI
            this.enableButton.IsEnabled = !enabled;
            this.stopButton.IsEnabled = enabled;
            this.statusCircle.Fill = enabled ? Brushes.Green : Brushes.Red;
            this.statusLabel.Content = enabled ? "Active" : "Not active";
            this.statusLabel.Content += !string.IsNullOrEmpty(labelContent) ? $" - {labelContent}" : "";
        }

        private void gamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.keyBindingsButton.IsEnabled = true;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            this.SetStatus(false, "Stopped");
        }

        private void keyBindingsButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
