using SymbolPad.Utils;
using SymbolPad.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace SymbolPad
{
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // Set the window style to not activate when clicked to behave like a virtual keyboard
            WindowActivator.SetNoActivate(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void ListBoxItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is SearchViewModel vm && sender is ListBoxItem item)
            {
                if (vm.SelectSymbolCommand.CanExecute(item.Content))
                {
                    vm.SelectSymbolCommand.Execute(item.Content);
                }
            }
        }

        private void SearchBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowActivator.ClearNoActivate(this);
            this.Activate();
            SearchBox.Focus();
        }

        private void SearchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            WindowActivator.SetNoActivate(this);
        }
    }
}