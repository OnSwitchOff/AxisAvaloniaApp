using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.ViewModels.SettingsSections;

namespace AxisAvaloniaApp.UserControls.Settings
{
    public partial class SpecialSettings : UserControl
    {
        public static readonly DirectProperty<SpecialSettings, SpecialSettingsViewModel> ViewModelProperty =
        AvaloniaProperty.RegisterDirect<SpecialSettings, SpecialSettingsViewModel>(
        nameof(ViewModel),
        o => o.ViewModel,
        (o, v) => o.ViewModel = v);

        private SpecialSettingsViewModel _ViewModel = new SpecialSettingsViewModel();

        public SpecialSettingsViewModel ViewModel
        {
            get { return _ViewModel; }
            set { SetAndRaise(ViewModelProperty, ref _ViewModel, value); }
        }

        public SpecialSettings()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
