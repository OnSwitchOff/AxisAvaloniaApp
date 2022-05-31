using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class InvoiceView : DocumentView
    {
        public InvoiceView()
        {
            InitializeComponent();
            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.InvoiceViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
