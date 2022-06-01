using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class ProformInvoiceView : DocumentView
    {
        public ProformInvoiceView() : base(Enums.ESerializationGroups.Invoice, Splat.Locator.Current.GetRequiredService<ViewModels.ProformInvoiceViewModel>())
        {

        }
    }
}
