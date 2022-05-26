using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class ProformInvoiceView : DocumentView
    {
        public ProformInvoiceView()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.ProformInvoiceViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
