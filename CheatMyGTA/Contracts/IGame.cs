using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Contracts
{
    public interface IGame
    {
        IGameData Data { get; set; }

        string ProcessName { get; set; }

        [JsonIgnore]
        string ProcessNameNoExtension { get; }
    }
}
