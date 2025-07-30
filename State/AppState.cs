using SymbolPad.Mvvm;

namespace SymbolPad.State
{
    // This class holds state shared across different ViewModels
    public class AppState : ObservableObject
    {
        private bool _isFullWidthMode;

        public bool IsFullWidthMode
        {
            get => _isFullWidthMode;
            set
            {
                if (_isFullWidthMode != value)
                {
                    _isFullWidthMode = value;
                    OnPropertyChanged(); // Notify all listeners (like MainWindow and SearchWindow)
                }
            }
        }
    }
}
