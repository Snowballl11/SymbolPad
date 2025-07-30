using System.Windows;

namespace SymbolPad
{
    public partial class CustomSymbolDialog : Window
    {
        public CustomSymbolDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
