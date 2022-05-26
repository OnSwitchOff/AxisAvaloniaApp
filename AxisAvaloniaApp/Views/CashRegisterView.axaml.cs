using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AxisAvaloniaApp.Views
{
    public partial class CashRegisterView : UserControl
    {
        public CashRegisterView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
