using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class CreditNoteView : DocumentView
    {
        public CreditNoteView() : base(Enums.ESerializationGroups.Invoice, Splat.Locator.Current.GetRequiredService<ViewModels.CreditNoteViewModel>())
        {

        }

    }
}
