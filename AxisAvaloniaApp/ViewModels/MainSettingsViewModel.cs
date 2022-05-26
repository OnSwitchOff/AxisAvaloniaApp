using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class MainSettingsViewModel : ReactiveObject
    {
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

        public MainSettingsViewModel()
        {
        }
    }
}
