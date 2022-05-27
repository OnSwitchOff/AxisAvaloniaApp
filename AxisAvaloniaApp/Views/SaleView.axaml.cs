using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using System;

namespace AxisAvaloniaApp.Views
{
    public partial class SaleView : UserControl
    {
        public SaleView()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.SaleViewModel>();
        }

        //private void TextBoxPartner_GotFocus(object? sender, GotFocusEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        private void TextBoxTitle_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.Focus();
                    break;
            }
        }

        private void TextBoxTitle_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case nameof(TextBox.IsVisible):
                    if (sender is TextBox textBox && textBox.IsVisible)
                    {
                        textBox.Focus();
                        textBox.CaretIndex = textBox.Text.Length;
                    }
                    break;
                case nameof(TextBox.IsFocused):
                    if (sender is TextBox tb && !tb.IsFocused)
                    {
                        (DataContext as ViewModels.SaleViewModel).IsSaleTitleReadOnly = true;
                    }
                    break;
            }
        }

        private void TextBoxPartner_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case (nameof(TextBox.IsFocused)):
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
