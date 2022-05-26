using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.UserControls.Extensions;
using AxisAvaloniaApp.ViewModels;

namespace AxisAvaloniaApp.UserControls.Settings
{
    public partial class DocumentSettings : UserControl
    {
        public static readonly DirectProperty<DocumentSettings, DocumentSettingsViewModel> ViewModelProperty =
           AvaloniaProperty.RegisterDirect<DocumentSettings, DocumentSettingsViewModel>(
           nameof(ViewModel),
           o => o.ViewModel,
           (o, v) => o.ViewModel = v);

        private DocumentSettingsViewModel _ViewModel = new DocumentSettingsViewModel();

        public DocumentSettingsViewModel ViewModel
        {
            get { return _ViewModel; }
            set { SetAndRaise(ViewModelProperty, ref _ViewModel, value); }
        }

        public DocumentSettings()
        {
            InitializeComponent();
            this.PropertyChanged += DocumentSettings_PropertyChanged;
        }

        private void DocumentSettings_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
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
