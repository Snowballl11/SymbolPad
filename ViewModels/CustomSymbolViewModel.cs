using SymbolPad.Mvvm;

namespace SymbolPad.ViewModels
{
    public class CustomSymbolViewModel : ObservableObject
    {
        private string _name = string.Empty;
        private string _halfWidth = string.Empty;
        private string _fullWidth = string.Empty;
        private bool _isPaired;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string HalfWidth { get => _halfWidth; set { _halfWidth = value; OnPropertyChanged(); } }
        public string FullWidth { get => _fullWidth; set { _fullWidth = value; OnPropertyChanged(); } }
        public bool IsPaired { get => _isPaired; set { _isPaired = value; OnPropertyChanged(); } }

        public CustomSymbolViewModel(Symbol? symbol = null)
        {
            if (symbol != null)
            {
                Name = symbol.Name;
                HalfWidth = symbol.HalfWidth;
                FullWidth = symbol.FullWidth;
                IsPaired = symbol.IsPaired;
            }
        }
    }
}
