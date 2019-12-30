using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Helpers
{
    public static class Constants
    {
        public const string ProcessNameRegex = @"^[\w\-. ]+.((exe)|(bin))$";

        public const string GameInfoLocation = "../../games.json";

        public const string KeyBindsLocation = "../../keyBinds.json";
    }
}
