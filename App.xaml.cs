using Hardcodet.Wpf.TaskbarNotification;
using SymbolPad.Services;
using SymbolPad.State;
using SymbolPad.ViewModels;
using System.Windows;
using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media;
using System.Linq;

namespace SymbolPad
{
    public partial class App : Application
    {
        private HotkeyService? _hotkeyService;
        private MainWindow? _mainWindow;
        private SearchWindow? _searchWindow;
        private TaskbarIcon? _taskbarIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Create Services and State
            var appState = new AppState();
            var symbolService = new SymbolService();
            var inputService = new InputService();

            // Load settings and apply theme
            var settings = StorageService.LoadSettings();
            appState.IsFullWidthMode = false; // Default state
            ApplyTheme(settings.IsDarkMode);

            // 2. Create ViewModels
            var mainViewModel = new MainViewModel(symbolService, inputService, appState);
            var searchViewModel = new SearchViewModel(symbolService, inputService, appState);

            // 3. Create Windows (Views)
            _mainWindow = new MainWindow { DataContext = mainViewModel };
            _searchWindow = new SearchWindow { DataContext = searchViewModel };
            // Settings window will be created on demand

            // 4. Setup System Tray Icon
            

            _taskbarIcon = new TaskbarIcon();
                        var iconStream = GetResourceStream(new Uri("pack://application:,,,/Assets/icon.ico")).Stream;
            _taskbarIcon.Icon = new System.Drawing.Icon(iconStream);
            _taskbarIcon.ToolTipText = "SymbolPad";

            var showMenuItem = new System.Windows.Controls.MenuItem { Header = "Show" };
            showMenuItem.Click += (s, e) => _mainWindow.Show();

            var contextMenu = new System.Windows.Controls.ContextMenu();
            var settingsMenuItem = new System.Windows.Controls.MenuItem { Header = "Settings" };
            settingsMenuItem.Click += (s, e) =>
            {
                var settingsViewModel = new SettingsViewModel(symbolService);
                var settingsWindow = new SettingsWindow
                {
                    DataContext = settingsViewModel
                };
                settingsWindow.Show();
            };
            var aboutMenuItem = new System.Windows.Controls.MenuItem { Header = "About" };
            aboutMenuItem.Click += (s, e) =>
            {
                var aboutWindow = new AboutWindow();
                aboutWindow.Show();
            };

            var exitMenuItem = new System.Windows.Controls.MenuItem { Header = "Exit" };
            exitMenuItem.Click += (s, e) => Shutdown();

            contextMenu.Items.Add(showMenuItem);
            contextMenu.Items.Add(settingsMenuItem);
            contextMenu.Items.Add(aboutMenuItem);
            contextMenu.Items.Add(exitMenuItem);
            _taskbarIcon.ContextMenu = contextMenu;

            // 5. Setup Hotkeys
            _hotkeyService = new HotkeyService(_mainWindow);
            _hotkeyService.RegisterHotkeys();
            _hotkeyService.ShowMainWindowRequested += () => _mainWindow.Show();
            _hotkeyService.ShowSearchWindowRequested += () => _searchWindow.Show();

            // 6. Show About Window on Startup
            if (!settings.SilentLaunch)
            {
                var aboutWindowOnStartup = new AboutWindow();
                aboutWindowOnStartup.Show();
            }
        }

        public void ApplyTheme(bool isDark)
        {
            var themeUri = isDark ? "Themes/DarkTheme.xaml" : "Themes/LightTheme.xaml";
            var existingTheme = Resources.MergedDictionaries.FirstOrDefault(d => d.Source.OriginalString.Contains("Theme"));
            if (existingTheme != null)
            {
                Resources.MergedDictionaries.Remove(existingTheme);
            }
            Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(themeUri, UriKind.Relative) });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _hotkeyService?.Dispose();
            _taskbarIcon?.Dispose();
            base.OnExit(e);
        }
    }
}
