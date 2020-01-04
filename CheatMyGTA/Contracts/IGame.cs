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
        string Name { get; set; }

        string Process{ get; set; }

        [JsonIgnore]
        string ProcessName { get; }

        /// <summary>
        /// Key - cheatcode
        /// Value - description
        /// </summary>
        IReadOnlyDictionary<string, string> CheatCodes { get; }
    }
}
