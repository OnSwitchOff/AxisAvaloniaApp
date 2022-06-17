using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Activation;
using AxisAvaloniaApp.Services.Activation.ResponseModels;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public class MainSettingsViewModel : ReactiveObject, IVisible
    {
        private readonly IActivationService activationService;

        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        private double titleMinWidth;
        public double TitleMinWidth
        {
            get => titleMinWidth;
            set => this.RaiseAndSetIfChanged(ref titleMinWidth, value);
        }

        private string licenseCode;
        public string LicenseCode
        {
            get => licenseCode;
            set => this.RaiseAndSetIfChanged(ref licenseCode, value);
        }

        public ReactiveCommand<Unit, Unit> ActivateCommand { get; }

        public MainSettingsViewModel()
        {
            ActivateCommand = ReactiveCommand.Create(Activate);
            activationService = Splat.Locator.Current.GetRequiredService<IActivationService>();
        }

        private async void Activate()
        {
            var x1 = await activationService.GetStatus("123124");
            var x2 = await activationService.TryLicense("123124","123");
            var x3 = await activationService.GetLastVersion();

            var r2 = await x3.Content.ReadAsStringAsync();
            VersionResponse.GetLastVersion r = JsonConvert.DeserializeObject<VersionResponse.GetLastVersion>(r2);
        }
    }
}
