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
        private IBindingAgent bindingAgent;

        //Constructor
        public MainWindow()
        {
            InitializeComponent();

            //TODO: move those to app.xaml
            var games = GetGamesList();
            var gameKeyBinds = GetKeyBinds();

            this.bindingAgent = new BindingAgent(gameKeyBinds);
            this.keyboardListener = new KeyboardListener();

            keyboardListener.KeyDown += KeyboardListener_KeyDown;
        }

        //Wrap into new interface
        private Dictionary<string,Dictionary<Key, string>> GetKeyBinds()
        {
            using (StreamReader sr = new StreamReader(Constants.KeyBindsLocation))
            {
                var fileContent = sr.ReadToEnd();
                var gameKeyBinds = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<Key, string>>>(fileContent);

                sr.Close();

                return gameKeyBinds;
            }
        }

        private void KeyboardListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            //TODO: check if foreground window matches active process

            //Remove event listener so SendKeys.Send does not cause infinite recursion (st. overflow)
            keyboardListener.KeyDown -= KeyboardListener_KeyDown;

            var key = args.Key;
            var cheatCode = bindingAgent.GetCheatCode(key);

            //Binding agent should return "" or null when there is no cheat code on the specified key
            if(!string.IsNullOrEmpty(cheatCode))
            {
                SendKeys.SendWait(cheatCode);
            }

            //Reattach event listener
            keyboardListener.KeyDown += KeyboardListener_KeyDown;
        }

        private void InitButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (IGame)gamesComboBox.SelectedItem;

            if(selectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a game", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //Check whether process exists
            var process = Process.GetProcessesByName(selectedItem.ProcessNameNoExtension).FirstOrDefault();

            if (process == null)
            {
                System.Windows.Forms.MessageBox.Show($"Couldnt find the process {selectedItem.ProcessName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Set as active game
            bindingAgent.SetActive(selectedItem.Data);

            //Bring to front logic
            DialogResult result = System.Windows.Forms.MessageBox.Show("Process found. Go to Game?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Win32Methods.BringToFront(process);
            }
        }

        private IList<IGame> GetGamesList()
        {
            var games = new List<IGame>();

            using (StreamReader sr = new StreamReader(Constants.GameInfoLocation))
            {
                try
                {
                    var gamesToAdd = JsonConvert.DeserializeObject<List<Game>>(sr.ReadToEnd());

                    if (gamesToAdd != null) games.AddRange(gamesToAdd);
                    
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

            return games;
        }


        // Method to create games.json
        private void CreateSampleGamesJSON()
        {
            var games = new List<IGame>();
            Dictionary<string, string> cheats = new Dictionary<string, string>();
            cheats.Add("Weapons cheat #1", "nuttertools");
            Game game = new Game
            {
                Data = new GameData
                {
                    CheatCodes = cheats,
                    Name = "Notepad"
                },
                ProcessName = "notepad.exe"
            };
            games.Add(game);

            var json = JsonConvert.SerializeObject(games, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(Constants.GameInfoLocation))
            {
                sw.Write(json);
            }
        }

        // Method to create keyBinds.json
        private void CreateSampleKeyBindsJSON()
        {
            var gameKeyBinds = new Dictionary<string, Dictionary<Key, string>>();

            gameKeyBinds.Add("Notepad", new Dictionary<Key, string>());
            gameKeyBinds["Notepad"].Add(Key.K, "Weapons cheat #1");

            var json = JsonConvert.SerializeObject(gameKeyBinds, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(Constants.KeyBindsLocation))
            {
                sw.Write(json);
            }
        }
    }
}
