using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheatMyGTA.Contracts
{
    public interface ICheatBinder
    {
        IGame ActiveGame { get; set; }

        string GetCheatCode(Key key);
    }
}
