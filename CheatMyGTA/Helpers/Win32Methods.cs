using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Helpers
{
    public static class Win32Methods
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static void BringToFront(Process pTemp)
        {
            if(pTemp == null)
            {
                throw new ArgumentException($"{nameof(pTemp)} is null");
            }

            SetForegroundWindow(pTemp.MainWindowHandle);
        }
    }
}
