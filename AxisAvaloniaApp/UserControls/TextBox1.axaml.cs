using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

namespace AxisAvaloniaApp.UserControls
{
    public partial class TextBox1 : TextBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(TextBox);

        public TextBox1()
        {
            this.InitializeComponent();
            this.Text = "Test";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
