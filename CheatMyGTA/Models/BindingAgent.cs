using CheatMyGTA.Contracts;
using CheatMyGTA.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace CheatMyGTA.Models
{
    public class BindingAgent : IBindingAgent
    {
        private Dictionary<string, Dictionary<Key, string>> gameKeyBinds;
        private string activeGameName;

        public BindingAgent(ICollection<IGame> games)
        {
            CreateSampleJSON();

            var keyBinds = new Dictionary<Key, string>();
            //keyBinds = games.ToDictionary(k => k.Data.Name, v => v.Data.CheatCodes.ToDictionary()

            using(StreamReader sr = new StreamReader(Constants.KeyBindsLocation))
            {
                var fileContent = sr.ReadToEnd();

                gameKeyBinds = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<Key, string>>>(fileContent);
            }
        }

        public string GetCheatCode(Key key)
        {   
            if(activeGameName == null)
            {
                //TODO active game logic;
                MessageBox.Show("no active game");
                return "";
            }


            //TODO logic for key not bound (return "")
            return gameKeyBinds[activeGameName][key];
        }

        public void SetActive(IGameData gameData)
        {
            //TODO: check if gamedata is a key in dict
            this.activeGameName = gameData.Name;
        }

        private void CreateSampleJSON()
        {
            var gameKeyBinds = new Dictionary<string, Dictionary<Key, string>>();

            gameKeyBinds.Add("Notepad", new Dictionary<Key, string>());
            gameKeyBinds["Notepad"].Add(Key.K, "Weapons cheat #1");

            using(StreamWriter sw = new StreamWriter("../../keyBinds.json"))
            {
                var json = JsonConvert.SerializeObject(gameKeyBinds, Formatting.Indented);
                sw.Write(json);
            }
        }
    }
}
