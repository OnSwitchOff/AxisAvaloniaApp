using Avalonia.Controls;
using System;

namespace AxisAvaloniaApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnCloseClicked(object sender, EventArgs args)
        {
            Close();
        }

    }
}
