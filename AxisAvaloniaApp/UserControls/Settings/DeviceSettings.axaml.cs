using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.UserControls.Extensions;
using AxisAvaloniaApp.ViewModels;

namespace AxisAvaloniaApp.UserControls.Settings
{
    public partial class DeviceSettings : UserControl
    {

        public static readonly DirectProperty<DeviceSettings, DeviceSettingsViewModel> ViewModelProperty =
           AvaloniaProperty.RegisterDirect<DeviceSettings, DeviceSettingsViewModel>(
           nameof(ViewModel),
           o => o.ViewModel,
           (o, v) => o.ViewModel = v);

        private DeviceSettingsViewModel _ViewModel = new DeviceSettingsViewModel();

        public DeviceSettingsViewModel ViewModel
        {
            get { return _ViewModel; }
            set { SetAndRaise(ViewModelProperty, ref _ViewModel, value); }
        }

        public DeviceSettings()
        {
            InitializeComponent();
            this.PropertyChanged += DeviceSettings_PropertyChanged;
        }

        private void DeviceSettings_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Bounds" && this.Content != null)
            {
                RecalcMinTitleWidth();
            }
        }

        private void RecalcMinTitleWidth()
        {
            Grid grid = this.FindControl<Grid>("AxisCloudGrid");
            double minTitleWidth = 0;
            foreach (Control item in grid.Children)
            {
                if (item is AdaptiveSettingsItemControl)
                {
                    if (!((AdaptiveSettingsItemControl)item).IsVertical)
                    {
                        AxisTextBlock titleLabel = item.FindControl<AxisTextBlock>("tbLabel");
                        if (minTitleWidth < titleLabel.Bounds.Width)
                        {
                            minTitleWidth = titleLabel.Bounds.Width;
                        }
                    }
                }               
            }

            grid = this.FindControl<Grid>("POSTerminalGrid");
            foreach (Control item in grid.Children)
            {
                if (item is AdaptiveSettingsItemControl)
                {
                    if (!((AdaptiveSettingsItemControl)item).IsVertical)
                    {
                        AxisTextBlock titleLabel = item.FindControl<AxisTextBlock>("tbLabel");
                        if (minTitleWidth < titleLabel.Bounds.Width)
                        {
                            minTitleWidth = titleLabel.Bounds.Width;
                        }
                    }
                }
            }

            grid = this.FindControl<Grid>("AdaptiveSettingsGrid");
            foreach (Control item in grid.Children)
            {
                if (item is AdaptiveSettingsItemControl)
                {
                    if (!((AdaptiveSettingsItemControl)item).IsVertical)
                    {
                        AxisTextBlock titleLabel = item.FindControl<AxisTextBlock>("tbLabel");
                        if (minTitleWidth < titleLabel.Bounds.Width)
                        {
                            minTitleWidth = titleLabel.Bounds.Width;
                        }
                    }
                }
            }

            ViewModel.TitleMinWidth = minTitleWidth + 12;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
