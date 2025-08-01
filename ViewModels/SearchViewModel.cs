using SymbolPad.Mvvm;
using SymbolPad.Services;
using SymbolPad.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SymbolPad.ViewModels
{
    public class SearchViewModel : ObservableObject
    {
        private readonly SymbolService _symbolService;
        private readonly InputService _inputService;
        private List<Symbol>? _allSymbols;
        private string _searchText = string.Empty;
        
        public AppState AppState { get; }
        public ObservableCollection<Symbol> FilteredSymbols { get; } = new ObservableCollection<Symbol>();
        public ICommand SelectSymbolCommand { get; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterSymbols();
            }
        }

        public SearchViewModel(SymbolService symbolService, InputService inputService, AppState appState)
        {
            _symbolService = symbolService;
            _inputService = inputService;
            AppState = appState;

            LoadSymbols();

            _symbolService.OnSymbolsChanged += LoadSymbols;

            SelectSymbolCommand = new RelayCommand(ExecuteSelectSymbol);
        }

        private void LoadSymbols()
        {
            _allSymbols = _symbolService.GetSymbols();
            FilterSymbols(); // Initial load
        }

        private void FilterSymbols()
        {
            if (_allSymbols == null) return;

            FilteredSymbols.Clear();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var symbol in _allSymbols)
                {
                    FilteredSymbols.Add(symbol);
                }
            }
            else
            {
                var lowerCaseFilter = SearchText.ToLowerInvariant();
                var results = _allSymbols.Where(s =>
                    s.Name.ToLowerInvariant().Contains(lowerCaseFilter) ||
                    s.HalfWidth.Contains(lowerCaseFilter) ||
                    s.FullWidth.Contains(lowerCaseFilter));
                
                foreach(var symbol in results)
                {
                    FilteredSymbols.Add(symbol);
                }
            }
        }

        private void ExecuteSelectSymbol(object? parameter)
        {
            if (parameter is Symbol symbol)
            {
                string textToSend = AppState.IsFullWidthMode ? symbol.FullWidth : symbol.HalfWidth;
                if(string.IsNullOrEmpty(textToSend))
                {
                    textToSend = AppState.IsFullWidthMode ? symbol.HalfWidth : symbol.FullWidth;
                }
                
                _inputService.SendSymbol(textToSend, symbol.IsPaired);
                // The window should be hidden by the view/app logic after input
            }
        }
    }
}
