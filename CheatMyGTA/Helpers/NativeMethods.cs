using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CheatMyGTA.Helpers
{
    public class NativeMethods
    {
        public const uint WINEVENT_OUTOFCONTEXT = 0;
        public const uint EVENT_SYSTEM_FOREGROUND = 3;

        private IntPtr hook;
        private event WinEventDelegate windowChanged;

        public NativeMethods()
        {
            hook = IntPtr.Zero;
        }

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        public event WinEventDelegate WindowChanged
        {
            add
            {
                UnhookWinEvent(hook);
                windowChanged += value;
                hook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, windowChanged, 0, 0, WINEVENT_OUTOFCONTEXT);
            }
            remove
            {
                UnhookWinEvent(hook);
                windowChanged -= value;
                hook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, windowChanged, 0, 0, WINEVENT_OUTOFCONTEXT);
            }
        }

        public static uint GetProcessIdFromHandle(IntPtr hwnd)
        {
            uint processId = 0;
            GetWindowThreadProcessId(hwnd, out processId);

            return processId;
        }

        public static void BringToFront(Process process)
        {
            SetForegroundWindow(process.MainWindowHandle);
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        ~NativeMethods()
        {
            if(hook != IntPtr.Zero)
            {
                UnhookWinEvent(hook);
            }
        }
    }
}
