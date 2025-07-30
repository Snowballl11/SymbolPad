using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SymbolPad.Services
{
    public class HotkeyService : IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID_MAIN = 9000;
        private const int HOTKEY_ID_SEARCH = 9001;

        // Modifiers
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;

        // Keys
        private const uint VK_OEM_4 = 0xDB; // for '['
        private const uint VK_OEM_6 = 0xDD; // for ']'

        private HwndSource _source;
        private readonly Window _window; // An invisible or main window to hook into

        public event Action? ShowMainWindowRequested;
        public event Action? ShowSearchWindowRequested;

        public HotkeyService(Window window)
        {
            _window = window;
            var helper = new WindowInteropHelper(_window);
            helper.EnsureHandle(); // Ensure window handle is created
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
        }

        public void RegisterHotkeys()
        {
            var handle = new WindowInteropHelper(_window).Handle;
            if (!RegisterHotKey(handle, HOTKEY_ID_MAIN, MOD_CONTROL | MOD_SHIFT, VK_OEM_4))
            {
                // Handle registration failure
                MessageBox.Show("Failed to register hotkey Ctrl+Shift+[");
            }
            if (!RegisterHotKey(handle, HOTKEY_ID_SEARCH, MOD_CONTROL | MOD_SHIFT, VK_OEM_6))
            {
                // Handle registration failure
                MessageBox.Show("Failed to register hotkey Ctrl+Shift+]");
            }
        }

        public void UnregisterHotkeys()
        {
            var handle = new WindowInteropHelper(_window).Handle;
            UnregisterHotKey(handle, HOTKEY_ID_MAIN);
            UnregisterHotKey(handle, HOTKEY_ID_SEARCH);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                switch (wParam.ToInt32())
                {
                    case HOTKEY_ID_MAIN:
                        ShowMainWindowRequested?.Invoke();
                        handled = true;
                        break;
                    case HOTKEY_ID_SEARCH:
                        ShowSearchWindowRequested?.Invoke();
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }
        
        public void Dispose()
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotkeys();
        }

        // P/Invoke definitions
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
