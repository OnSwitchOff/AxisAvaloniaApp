using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class SaleView : UserControl
    {
        public SaleView()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.SaleViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
