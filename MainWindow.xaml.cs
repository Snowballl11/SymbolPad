using SymbolPad.Utils;
using System.ComponentModel;
using System.Windows;
using System;

namespace SymbolPad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // Set the window style to not activate when clicked
            WindowActivator.SetNoActivate(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Prevent the window from actually closing
            e.Cancel = true;
            // Hide the window instead
            this.Hide();
        }
    }
}
