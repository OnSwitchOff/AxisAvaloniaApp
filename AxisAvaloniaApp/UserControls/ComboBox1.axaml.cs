using AxisAvaloniaApp.Helpers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;
using System.Collections.Generic;

namespace AxisAvaloniaApp.UserControls
{
    public partial class ComboBox1 : ComboBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ComboBox);
        public ComboBox1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.Enter:
                    this.Items = (this.Items as IEnumerable<object>).Add(this.PlaceholderText);
                    break;
                case Key.Back:
                    this.PlaceholderText = this.PlaceholderText.Substring(0, this.PlaceholderText.Length - 1);
                    break;
            }

            if (e.Key == Key.Back)
            {
                
                
            }

            e.Handled = e.Key == Key.Enter || e.Key == Key.Back;
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            this.PlaceholderText += e.Text;
            base.OnTextInput(e);
        }
    }
}
