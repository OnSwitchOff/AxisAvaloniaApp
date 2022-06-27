using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Activation;
using AxisAvaloniaApp.Services.Activation.ResponseModels;
using AxisAvaloniaApp.Services.Printing;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public class MainSettingsViewModel : ReactiveObject, IVisible
    {
        private readonly IActivationService activationService;
        private readonly IPrintService printService;

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
            printService = Splat.Locator.Current.GetRequiredService<IPrintService>();
        }

        private async void Activate()
        {
            List<Image> list = new List<Image>();
            string img = "C:\\Users\\viktor.kassov\\Desktop\\фывф.png";
            Image image = Image.FromFile(img);
            list.Add(image);
            var p = printService.GetPrinters();

            bool pr = printService.PrintImageList(p[4], list,false);


            var x1 = await activationService.GetStatus("8714536025");
           // x1.EnsureSuccessStatusCode();
            var r1 = await x1.Content.ReadAsStringAsync();

            var x2 = await activationService.TryLicense("8714536025", "123");
            var r2 = await x2.Content.ReadAsStringAsync();
            var x3 = await activationService.GetLastVersion();
            x3.EnsureSuccessStatusCode();
            var r3 = await x3.Content.ReadAsStringAsync();            
            VersionResponse.GetLastVersion r = JsonConvert.DeserializeObject<VersionResponse.GetLastVersion>(r3);
        }
    }
}
