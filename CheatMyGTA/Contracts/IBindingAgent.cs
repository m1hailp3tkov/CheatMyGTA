using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheatMyGTA.Contracts
{
    public interface IBindingAgent
    {
        void SetActive(IGameData gameData);

        string GetCheatCode(Key key);

        void BindKey(Key key, string cheatCodeName);

        void UnbindKey(Key key);
    }
}
