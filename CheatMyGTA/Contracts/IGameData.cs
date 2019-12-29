using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Contracts
{
    public interface IGameData
    {
        IDictionary<string, string> CheatCodes { get; set; }

        string Name { get; set; }

        string ProcessName { get; set; }

        string ProcessNameNoExtension { get; }
    }
}
