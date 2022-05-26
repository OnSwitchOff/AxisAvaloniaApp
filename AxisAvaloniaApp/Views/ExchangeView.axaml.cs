using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AxisAvaloniaApp.Views
{
    public partial class ExchangeView : UserControl
    {
        public ExchangeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
