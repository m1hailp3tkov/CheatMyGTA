using CheatMyGTA.Contracts;
using CheatMyGTA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheatMyGTA.Models
{
    public class GameData : IGameData
    {
        public GameData()
        {
            this.CheatCodes = new Dictionary<string, string>();
        }

        public GameData(IDictionary<string,string> cheatCodes)
        {
            this.CheatCodes = cheatCodes;
        }

        public string Name { get; set; }

        public IDictionary<string,string> CheatCodes { get; set; }
    }
}
