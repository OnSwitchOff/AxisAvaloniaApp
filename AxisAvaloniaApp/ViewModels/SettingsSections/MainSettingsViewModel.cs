using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public class MainSettingsViewModel : ReactiveObject, IVisible
    {
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

        public MainSettingsViewModel()
        {
        }
    }
}
