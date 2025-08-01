using SymbolPad.Mvvm;
using SymbolPad.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SymbolPad.State; // Add this using

namespace SymbolPad.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly SymbolService _symbolService;
        private readonly InputService _inputService;
        public AppState AppState { get; } // Expose AppState to the View

        public ObservableCollection<Symbol> TopSymbols { get; } = new ObservableCollection<Symbol>();
        public ICommand InputSymbolCommand { get; }
        public ICommand ToggleWidthModeCommand { get; }

        public MainViewModel(SymbolService symbolService, InputService inputService, AppState appState)
        {
            _symbolService = symbolService;
            _inputService = inputService;
            AppState = appState;

            LoadSymbols();

            _symbolService.OnSymbolsChanged += LoadSymbols;

            InputSymbolCommand = new RelayCommand(ExecuteInputSymbol);

            ToggleWidthModeCommand = new RelayCommand(_ =>
            {
                _inputService.ToggleFullWidthMode();
                AppState.IsFullWidthMode = !AppState.IsFullWidthMode;
            });
        }

        private void LoadSymbols()
        {
            TopSymbols.Clear();
            var allSymbols = _symbolService.GetSymbols();
            foreach (var symbol in allSymbols)
            {
                TopSymbols.Add(symbol);
            }
        }

        private void ExecuteInputSymbol(object? parameter)
        {
            if (parameter is Symbol symbol)
            {
                // Read the mode from the shared AppState
                string textToSend = AppState.IsFullWidthMode ? symbol.FullWidth : symbol.HalfWidth;
                if(string.IsNullOrEmpty(textToSend)) // Fallback if one form is empty
                {
                    textToSend = AppState.IsFullWidthMode ? symbol.HalfWidth : symbol.FullWidth;
                }
                
                _inputService.SendSymbol(textToSend, symbol.IsPaired);
                // The window should be hidden by the view/app logic after input
            }
        }
    }
}
