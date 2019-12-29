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

        public string Name { get; set; }

        public string ProcessName { get; set; }

        public string ProcessNameNoExtension { get => this.ProcessName.Split('.')[0]; }

        public IDictionary<string,string> CheatCodes { get; set; }

        public override string ToString()
        {
            return $"{this.Name} ({this.ProcessName})";
        }
    }
}
