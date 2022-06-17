using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Activation;
using AxisAvaloniaApp.Views;
using Splat;

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

            IActivationService activationService = Locator.Current.GetRequiredService<IActivationService>();
            activationService.ActivateAsync();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                MainWindow = desktop.MainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        public static Avalonia.Controls.Window MainWindow { get; private set; }
    }
}
