using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheatMyGTA.Contracts
{
    public interface IKeyBinds
    {
        IReadOnlyDictionary<Key, string> GetKeyBinds(IGame game);

        void SetKeyBinds(IGame game, IDictionary<Key, string> keyBinds);

        void Save();
    }
}
