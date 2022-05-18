using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AxisAvaloniaApp.UserControls
{
    public partial class AutocompleteInput : UserControl
    {

        public static readonly DirectProperty<AutocompleteInput, string> TextInputProperty =
        AvaloniaProperty.RegisterDirect<AutocompleteInput, string>(
        nameof(TextInput),
        o => o.TextInput,
        (o, v) => o.TextInput = v);

        private string  textInput = string.Empty;

        public string TextInput
        {
            get { return textInput; }
            set { SetAndRaise(TextInputProperty, ref textInput, value); }
        }

        public AutocompleteInput()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
