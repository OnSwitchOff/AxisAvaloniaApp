using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace AxisAvaloniaApp.Views
{
    public partial class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            InitializeComponent();

            //Avalonia.Controls.Html.HtmlControl control = new Avalonia.Controls.Html.HtmlControl();
            //control.

            Avalonia.Controls.Html.HtmlLabel label = this.FindControl<Avalonia.Controls.Html.HtmlLabel>("htmlLabel");
            string appPath = System.IO.Path.GetDirectoryName(Environment.CurrentDirectory).Replace("\\bin\\Debug", "\\Assets\\Icons\\hamburgerButton_Expand.png");
            label.Text = string.Format(
                "<!DOCTYPE html><html><head><title>Page Title</title></head><body><h1>This is a Heading</h1><p>This is a paragraph.</p><img src = {0}></body></html>",
                appPath);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
