using SymbolPad.Mvvm;
using SymbolPad.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SymbolPad.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly SymbolService _symbolService;
        private Symbol? _selectedSymbol;
        private bool _isDarkMode;

        public ObservableCollection<Symbol> Symbols { get; }
        public ICommand AddSymbolCommand { get; }
        public ICommand EditSymbolCommand { get; }
        public ICommand DeleteSymbolCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand SaveCommand { get; }

        public Symbol? SelectedSymbol { get => _selectedSymbol; set { _selectedSymbol = value; OnPropertyChanged(); } }
        public bool IsDarkMode { get => _isDarkMode; set { _isDarkMode = value; OnPropertyChanged(); } }

        public SettingsViewModel(SymbolService symbolService)
        {
            _symbolService = symbolService;
            var settings = StorageService.LoadSettings(); // Load current settings
            IsDarkMode = settings.IsDarkMode;
            Symbols = new ObservableCollection<Symbol>(_symbolService.GetSymbols());
            
            AddSymbolCommand = new RelayCommand(_ => AddSymbol());
            EditSymbolCommand = new RelayCommand(EditSymbol, CanEditOrDelete);
            DeleteSymbolCommand = new RelayCommand(DeleteSymbol, CanEditOrDelete);
            MoveUpCommand = new RelayCommand(MoveUp, CanMoveUp);
            MoveDownCommand = new RelayCommand(MoveDown, CanMoveDown);
            SaveCommand = new RelayCommand(SaveChanges);
        }

        // Methods for Add, Edit, Delete, MoveUp, MoveDown...
        private void AddSymbol()
        {
            var viewModel = new CustomSymbolViewModel();
            var dialog = new CustomSymbolDialog
            {
                DataContext = viewModel
            };

            if (dialog.ShowDialog() == true)
            {
                var newSymbol = new Symbol
                {
                    Name = viewModel.Name,
                    HalfWidth = viewModel.HalfWidth,
                    FullWidth = viewModel.FullWidth,
                    IsPaired = viewModel.IsPaired,
                    IsDefault = false // Custom symbols are not default
                };
                Symbols.Add(newSymbol);
            }
        }
        private void EditSymbol(object? p)
        {
            if (SelectedSymbol == null) return;

            var viewModel = new CustomSymbolViewModel(SelectedSymbol);
            var dialog = new CustomSymbolDialog
            {
                DataContext = viewModel
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedSymbol.Name = viewModel.Name;
                SelectedSymbol.HalfWidth = viewModel.HalfWidth;
                SelectedSymbol.FullWidth = viewModel.FullWidth;
                SelectedSymbol.IsPaired = viewModel.IsPaired;
            }
        }

        private void DeleteSymbol(object? p)
        {
            if (SelectedSymbol != null)
            {
                Symbols.Remove(SelectedSymbol);
            }
        }
        private void MoveUp(object? parameter)
        {
            if (SelectedSymbol != null)
            {
                var index = Symbols.IndexOf(SelectedSymbol);
                if (index > 0)
                {
                    Symbols.Move(index, index - 1);
                }
            }
        }

        private void MoveDown(object? parameter)
        {
            if (SelectedSymbol != null)
            {
                var index = Symbols.IndexOf(SelectedSymbol);
                if (index < Symbols.Count - 1)
                {
                    Symbols.Move(index, index + 1);
                }
            }
        }
        private bool CanEditOrDelete(object? p) => SelectedSymbol != null && !SelectedSymbol.IsDefault;
        private bool CanMoveUp(object? p) => SelectedSymbol != null && Symbols.IndexOf(SelectedSymbol) > 0;
        private bool CanMoveDown(object? p) => SelectedSymbol != null && Symbols.IndexOf(SelectedSymbol) < Symbols.Count - 1;

        private void SaveChanges(object? parameter)
        {
            var newSettings = new UserSettings
            {
                IsDarkMode = this.IsDarkMode,
                SymbolOrder = new System.Collections.Generic.List<System.Guid>(Symbols.Select(s => s.Id)),
                CustomSymbols = new System.Collections.Generic.List<Symbol>(Symbols.Where(s => !s.IsDefault))
            };
            _symbolService.SaveSettings(newSettings);
            
            // Logic to close the settings window, usually handled by the View
        }
    }
}
