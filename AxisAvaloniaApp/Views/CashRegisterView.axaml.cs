using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.ViewModels;
using System;

namespace AxisAvaloniaApp.Views
{
    public partial class CashRegisterView : UserControl
    {
        public CashRegisterView()
        {
            InitializeComponent();
            DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.CashRegisterViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
