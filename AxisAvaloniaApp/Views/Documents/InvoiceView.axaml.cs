using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class InvoiceView : DocumentView
    {
        public InvoiceView():base(Enums.ESerializationGroups.Invoice, Splat.Locator.Current.GetRequiredService<ViewModels.InvoiceViewModel>())
        {
            //InitializeComponent();           
        }

        //private void InitializeComponent()
        //{
        //    AvaloniaXamlLoader.Load(this);
        //}


        
    }
}
