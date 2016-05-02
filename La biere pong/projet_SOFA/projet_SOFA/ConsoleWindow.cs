using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace AtelierXNA
{
    class ConsoleWindow
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();  
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
            Console.WriteLine("*** Ne pas effacer la console -l'application fermera- ***");
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
        }

        public static void PutInForeGround()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr hWnd = currentProcess.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
                ShowWindow(hWnd, SW_SHOW);
            }
        }
    }
}
