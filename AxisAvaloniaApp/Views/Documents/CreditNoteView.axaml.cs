using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.Views
{
    public partial class CreditNoteView : DocumentView
    {
        public CreditNoteView()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.CreditNoteViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
