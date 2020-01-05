using CheatMyGTA.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheatMyGTA.Services
{
    public class CheatBinder : ICheatBinder
    {
        private readonly IGameSource gameSource;
        private readonly IKeyBinds keyBinds;

        public CheatBinder(IGameSource gameSource, IKeyBinds keyBinds)
        {
            this.gameSource = gameSource;
            this.keyBinds = keyBinds;
        }

        public IGame ActiveGame { get; set; }

        public string GetCheatCode(Key key)
        {
            var gameKeyBinds = keyBinds.GetKeyBinds(ActiveGame);

            if(gameKeyBinds.ContainsKey(key))
            {
                return gameKeyBinds[key];
            }

            return "";
        }
    }
}
