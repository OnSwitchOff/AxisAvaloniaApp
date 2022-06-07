using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.UserControls.Extensions;
using AxisAvaloniaApp.ViewModels.Settings;
using System.Diagnostics;

namespace AxisAvaloniaApp.UserControls.Settings
{
    public partial class ObjectSettings : UserControl
    {
        public static readonly DirectProperty<ObjectSettings, ObjectSettingsViewModel> ViewModelProperty =
        AvaloniaProperty.RegisterDirect<ObjectSettings, ObjectSettingsViewModel>(
        nameof(ViewModel),
        o => o.ViewModel,
        (o, v) => o.ViewModel = v);

        private ObjectSettingsViewModel _ViewModel = new ObjectSettingsViewModel();

        public ObjectSettingsViewModel ViewModel
        {
            get { return _ViewModel; }
            set { SetAndRaise(ViewModelProperty, ref _ViewModel, value); }
        }

        public ObjectSettings()
        {
            InitializeComponent();
            this.PropertyChanged += ObjectSettings_PropertyChanged;
        }

        private void ObjectSettings_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Bounds" && this.Content != null)
            {
                RecalcMinTitleWidth();
            }
        }

        private void RecalcMinTitleWidth()
        {
            Grid grid = this.FindControl<Grid>("AdaptiveSettingsGrid");
            double minTitleWidth = 0;
            foreach (AdaptiveSettingsItemControl item in grid.Children)
            {
                if (!item.IsVertical)
                {
                    AxisTextBlock titleLabel = item.FindControl<AxisTextBlock>("tbLabel");
                    if (minTitleWidth < titleLabel.Bounds.Width)
                    {
                        minTitleWidth = titleLabel.Bounds.Width;
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
