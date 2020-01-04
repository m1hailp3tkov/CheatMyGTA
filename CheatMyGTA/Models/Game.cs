using CheatMyGTA.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Models
{
    public class Game : IGame
    {
        private Dictionary<string,string> cheatCodes;

        public Game(Dictionary<string, string> cheatCodes)
        {
            this.CheatCodes = cheatCodes;
        }

        public string Name { get; set; }
        public string Process { get; set; }

        public string ProcessName => this.Process.Split('.')[0];

        public IReadOnlyDictionary<string, string> CheatCodes
        {
            //TODO: return actual readonly dictionary
            get => new ReadOnlyDictionary<string,string>(cheatCodes);
            private set => cheatCodes = value.ToDictionary(k => k.Key, v => v.Value);
        }

        public override string ToString()
        {
            return $"{Name} ({Process})";
        }
    }
}
