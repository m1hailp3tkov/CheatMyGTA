using CheatMyGTA.Models;
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
using System.Windows.Forms;
using System.Threading;
using CheatMyGTA.Helpers;
using CheatMyGTA.Contracts;

namespace CheatMyGTA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyboardListener keyboardListener;
        private List<IGameData> games;

        public MainWindow()
        {
            InitializeComponent();

            //TODO: move those to app.xaml

            games = new List<IGameData>();
            LoadGamesList();

            this.keyboardListener = new KeyboardListener();
        }

        private void KeyboardListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            //keyboardListener.KeyDown -= KeyboardListener_KeyDown;

            //SendKeys.SendWait("hello");

            //keyboardListener.KeyDown += KeyboardListener_KeyDown;
        }

        private void InitButton_Click(object sender, RoutedEventArgs e)
        {
            string processName = ((IGameData)gamesComboBox.SelectedItem).ProcessNameNoExtension;
            var process = Process.GetProcessesByName(processName).FirstOrDefault();

            if (process == null)
            {
                System.Windows.Forms.MessageBox.Show($"Couldnt find the process {processName}.exe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = System.Windows.Forms.MessageBox.Show("Process found. Go to Game?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if(result == System.Windows.Forms.DialogResult.Yes)
            {
                Win32Methods.BringToFront(process);
            }
        }

        private void LoadGamesList()
        {
            using (StreamReader sr = new StreamReader(Constants.GameInfoLocation))
            {
                try
                {
                    var gamesToAdd = JsonConvert.DeserializeObject<List<GameData>>(sr.ReadToEnd());

                    if(gamesToAdd != null) games.AddRange(gamesToAdd);

                    if (gamesToAdd.Count == 0) System.Windows.Forms.MessageBox.Show("No gamedata found in games.json.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("games.json does not exist or is corrupt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            foreach (var game in games)
            {
                gamesComboBox.Items.Add(game);
            }
        }
    }
}
