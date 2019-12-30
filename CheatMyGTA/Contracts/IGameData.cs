﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Contracts
{
    public interface IGameData
    {
        string Name { get; set; }

        IDictionary<string, string> CheatCodes { get; set; }
    }
}
