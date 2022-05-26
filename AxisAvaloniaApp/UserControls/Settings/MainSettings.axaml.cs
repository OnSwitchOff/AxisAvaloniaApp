using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.ViewModels;

namespace AxisAvaloniaApp.UserControls.Settings
{
    public partial class MainSettings : UserControl
    {
        public static readonly DirectProperty<MainSettings, MainSettingsViewModel> ViewModelProperty =
               AvaloniaProperty.RegisterDirect<MainSettings, MainSettingsViewModel>(
               nameof(ViewModel),
               o => o.ViewModel,
               (o, v) => o.ViewModel = v);

        private MainSettingsViewModel _ViewModel = new MainSettingsViewModel();

        public MainSettingsViewModel ViewModel
        {
            get { return _ViewModel; }
            set { SetAndRaise(ViewModelProperty, ref _ViewModel, value); }
        }
        public MainSettings()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
