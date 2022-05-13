using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AxisAvaloniaApp.Helpers;
using System;

namespace AxisAvaloniaApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.MainWindowViewModel>();

            //var asset = Avalonia.AvaloniaLocator.Current.GetService<Avalonia.Platform.IAssetLoader>();
            //var logo = new System.Drawing.Icon(stream: asset.Open(new Uri(@"avares://AxisAvaloniaApp/Assets/Icons/sale.png")));
        }

        public void OnCloseClicked(object sender, EventArgs args)
        {
            Close();
        }

    }
}
