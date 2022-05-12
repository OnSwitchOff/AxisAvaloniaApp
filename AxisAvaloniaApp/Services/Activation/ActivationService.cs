using CommunityToolkit.Mvvm.DependencyInjection;
using System.Threading.Tasks;
using AxisAvaloniaApp.Services.AxisCloud;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.ThemeSelector;
using AxisAvaloniaApp.Services.Translation;

namespace AxisAvaloniaApp.Services.Activation
{
    public class ActivationService : IActivationService
    {
        private readonly ISettingsService settings;
        private readonly IThemeSelectorService themeSelectorService;

        //private UIElement shell = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationService"/> class.
        /// </summary>
        /// <param name="settings">Settings of the app.</param>
        /// <param name="themeSelectorService">Service to set theme of the app.</param>
        public ActivationService(ISettingsService settings, IThemeSelectorService themeSelectorService)
        {
            this.settings = settings;
            this.themeSelectorService = themeSelectorService;
        }

        public async Task ActivateAsync()
        {
            // Initialize services that you need before app activation
            // take into account that the splash screen is shown while this code runs.
            await InitializeAsync();

            //if (App.MainWindow.Content is null)
            //{
            //    shell = Ioc.Default.GetService<MainView>();
            //    App.MainWindow.Content = shell ?? new Frame();
            //}

            //// Ensure the current window is active
            //App.MainWindow.Activate();

            // Tasks after activation
            await StartupAsync();
        }

        private async Task InitializeAsync()
        {
            //await themeSelectorService.InitializeAsync().ConfigureAwait(false);
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            IAxisCloudService axisCloudService = Ioc.Default.GetService<IAxisCloudService>();
            if ((bool)this.settings.AxisCloudSettings[Enums.ESettingKeys.DeviceIsUsed])
            {
                axisCloudService.StartServiceAsync((int)this.settings.AxisCloudSettings[Enums.ESettingKeys.ComPort], this.settings);
            }

            //await themeSelectorService.SetRequestedThemeAsync();
            await Task.CompletedTask;
        }
    }
}
