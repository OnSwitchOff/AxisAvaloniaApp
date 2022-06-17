using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.StartUp;
using AxisAvaloniaApp.Views;
using Splat;
using System.ComponentModel;

namespace AxisAvaloniaApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        [System.Obsolete]
        public override void OnFrameworkInitializationCompleted()
        {
            // регистрируем зависимости (сервисы)
            Services.Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);            

            IStartUpService startUpService = Locator.Current.GetRequiredService<IStartUpService>();
            startUpService.ActivateAsync();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                MainWindow = desktop.MainWindow;
                MainWindow.Closing += MainWindow_Closing;
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Gets main window.
        /// </summary>
        /// <date>16.06.2022.</date>
        public static Avalonia.Controls.Window MainWindow { get; private set; }

        /// <summary>
        /// Closes services when window is closing.
        /// </summary>
        /// <param name="sender">Window.</param>
        /// <param name="e">CancelEventArgs.</param>
        /// <date>17.06.2022.</date>
        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            Services.Scanning.IScanningData scanningData = Locator.Current.GetRequiredService<Services.Scanning.IScanningData>();
            scanningData.StopCOMScanner();
            Services.Payment.IPaymentService paymentService = Locator.Current.GetRequiredService<Services.Payment.IPaymentService>();
            paymentService.Dispose();
            Services.AxisCloud.IAxisCloudService axisCloudService = Locator.Current.GetRequiredService<Services.AxisCloud.IAxisCloudService>();
            axisCloudService.StopService();
        }
    }
}
