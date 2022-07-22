using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.StartUp;
using AxisAvaloniaApp.Views;
using Splat;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AxisAvaloniaApp
{
    public partial class App : Application
    {
        public static Avalonia.Threading.DispatcherTimer OfflineTimer { get; set; }
        public static DateTime OfflineStartDate { get; set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        [System.Obsolete]
        public override async void OnFrameworkInitializationCompleted()
        {
            Avalonia.Media.Imaging.Bitmap bitmap = new Avalonia.Media.Imaging.Bitmap(@"C:\Users\serhii.rozniuk\Desktop\fiscalDeviceLogo.png");
            using (System.IO.Stream stream = new System.IO.MemoryStream())
            {
                bitmap.Save(stream);
            }
            // регистрируем зависимости (сервисы)
            Services.Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            { 
                MainWindow mw = null;
                SplashScreenView sw = null;
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;

                if (!Configurations.AppConfiguration.IsDatabaseExist)
                {
                    LocalizationView dialog = new LocalizationView();
                    sw = await dialog.MyShowDialog();
                }

                if (sw == null)
                {
                    sw = new SplashScreenView();
                }
                mw = await sw.MyShowDialog();

                OfflineTimer = new Avalonia.Threading.DispatcherTimer();
                desktop.MainWindow = mw == null ? new MainWindow() : mw;
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
