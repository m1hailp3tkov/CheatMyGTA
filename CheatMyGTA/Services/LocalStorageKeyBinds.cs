using CheatMyGTA.Contracts;
using CheatMyGTA.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheatMyGTA.Services
{
    public class LocalStorageKeyBinds : IKeyBinds
    {
        private Dictionary<string, Dictionary<Key, string>> gamesKeyBinds;

        public LocalStorageKeyBinds()
        {
            using (StreamReader sr = new StreamReader(Constants.KeyBindsLocation))
            {
                var jsonContent = sr.ReadToEnd();

                this.gamesKeyBinds = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<Key, string>>>(jsonContent);
            }
        }

        public IReadOnlyDictionary<Key, string> GetKeyBinds(IGame game)
        {
            //TODO: actual readonly Dictionary
            return this.gamesKeyBinds[game.Name];
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
