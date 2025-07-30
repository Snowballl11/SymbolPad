using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SymbolPad.Utils
{
    public static class WindowActivator
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        public static void SetNoActivate(Window window)
        {
            var helper = new WindowInteropHelper(window);
            var hwnd = helper.EnsureHandle();
            SetWindowLong(hwnd, GWL_EXSTYLE, GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
    }
}
