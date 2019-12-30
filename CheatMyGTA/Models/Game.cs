using CheatMyGTA.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Models
{
    public class Game : IGame
    {
        public Game()
        {
            this.Data = new GameData();
        }

        public IGameData Data { get; set; }

        public string ProcessName { get; set; }

        public string ProcessNameNoExtension { get => this.ProcessName.Split('.')[0]; }

        public override string ToString()
        {
            return $"{this.Data.Name} ({this.ProcessName})";
        }
    }
}
