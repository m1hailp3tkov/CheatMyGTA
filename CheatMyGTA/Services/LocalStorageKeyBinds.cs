using CheatMyGTA.Contracts;
using CheatMyGTA.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if (game == null) throw new ArgumentNullException(nameof(game));

            if(!gamesKeyBinds.ContainsKey(game.Name))
            {
                this.gamesKeyBinds.Add(game.Name, new Dictionary<Key, string>());
            }
                
            return new ReadOnlyDictionary<Key, string>(this.gamesKeyBinds[game.Name]);
        }

        public void SetKeyBinds(IGame game, IDictionary<Key, string> keyBinds)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (keyBinds == null) throw new ArgumentNullException(nameof(keyBinds));

            if(keyBinds.Count == 0)
            {
                this.gamesKeyBinds.Remove(game.Name);
                return;
            }

            this.gamesKeyBinds[game.Name] = (Dictionary<Key,string>)keyBinds;
        }

        public void Save()
        {
            var ordered = this.gamesKeyBinds
                .OrderBy(x => x.Key)
                .ToDictionary(
                                key => key.Key, 
                                value => value.Value
                                    .OrderBy(x => x.Key)
                                    .ToDictionary(x => x.Key, y => y.Value));


            var jsonContent = JsonConvert.SerializeObject(ordered, Formatting.Indented);

            using(StreamWriter sw = new StreamWriter(Constants.KeyBindsLocation))
            {
                sw.WriteLine(jsonContent);
            }
        }
    }
}
