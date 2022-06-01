using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class DebitNoteView : DocumentView
    {
        public DebitNoteView() : base(Enums.ESerializationGroups.Invoice, Splat.Locator.Current.GetRequiredService<ViewModels.DebitNoteViewModel>())
        {

        }

    }
}
