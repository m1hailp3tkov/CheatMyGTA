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

        //TODO: wrap in an object
        public BindingAgent(Dictionary<string, Dictionary<Key, string>> gameKeyBinds)
        {
            this.gameKeyBinds = gameKeyBinds;
        }

        public void BindKey(Key key, string cheatCodeName)
        {
            if(gameKeyBinds[activeGameName].ContainsKey(key))
            {
                //TODO: Extension class
                throw new ApplicationException($"[{key}] is already bound for {gameKeyBinds[activeGameName][key]}");
            }

            gameKeyBinds[activeGameName].Add(key, cheatCodeName);
        }

        public string GetCheatCode(Key key)
        {   
            if(activeGameName == null)
            {
                return "";
            }

            if(gameKeyBinds[activeGameName].ContainsKey(key))
            {
                return gameKeyBinds[activeGameName][key];
            }

            return "";
        }

        public void SetActive(IGameData gameData)
        {
            if(string.IsNullOrEmpty(gameData.Name) || string.IsNullOrWhiteSpace(gameData.Name))
            {
                throw new ArgumentException($"{nameof(gameData)} does not contain a valid {nameof(gameData.Name)} property.");
            }

            if(!gameKeyBinds.ContainsKey(gameData.Name))
            {
                throw new ArgumentException($"{gameData.Name} does not persist in {nameof(gameKeyBinds)} dictionary.");
            }

            this.activeGameName = gameData.Name;
        }

        public void UnbindKey(Key key)
        {
            if (!gameKeyBinds[activeGameName].ContainsKey(key))
            {
                //TODO: Extension class
                throw new ApplicationException($"[{key}] is not bound anywhere");
            }

            gameKeyBinds[activeGameName].Remove(key);
        }
    }
}
