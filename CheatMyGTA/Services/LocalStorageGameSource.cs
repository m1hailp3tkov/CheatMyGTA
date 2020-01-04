using CheatMyGTA.Contracts;
using CheatMyGTA.Helpers;
using CheatMyGTA.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Services
{
    public class LocalStorageGameSource : IGameSource
    {
        private List<IGame> gameList;

        public IReadOnlyList<IGame> GameList => gameList.AsReadOnly();

        public void Load()
        {
            using (StreamReader sr = new StreamReader(Constants.GameInfoLocation))
            {
                var jsonContent = sr.ReadToEnd();

                var games = JsonConvert.DeserializeObject<List<Game>>(jsonContent)
                    .OrderBy(g => g.Name);

                this.gameList = new List<IGame>(games);
            }
        }
    }
}
