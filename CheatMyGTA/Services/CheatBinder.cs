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
        private IGame activeGame;
        private readonly IGameSource gameSource;
        private readonly IKeyBinds keyBinds;
        private IDictionary<Key, string> gameKeyBinds;

        public CheatBinder(IGameSource gameSource, IKeyBinds keyBinds)
        {
            this.gameSource = gameSource;
            this.keyBinds = keyBinds;
        }

        public IGame ActiveGame
        {
            get => this.activeGame;
            set
            {
                this.activeGame = value;
                var binds = (IDictionary<Key, string>)this.keyBinds.GetKeyBinds(activeGame);
                this.gameKeyBinds = new Dictionary<Key, string>(binds);
            }
        }

        public string GetCheatCode(Key key)
        {
            if(gameKeyBinds.ContainsKey(key))
            {
                return gameKeyBinds[key];
            }

            return "";
        }

        public void SetCheatCode(Key key, string cheatCode)
        {
            
        }
    }
}
