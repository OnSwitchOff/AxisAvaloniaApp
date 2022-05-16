using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;

namespace AxisAvaloniaApp.UserControls.NavigationView
{
    public class NavigationViewItem : TemplatedControl, ISelectable, IDataTemplate
    {

        public NavigationViewItem()
        {
            AddHandler(PointerPressedEvent, NavigationViewItem_PointerPressed, Avalonia.Interactivity.RoutingStrategies.Tunnel);
        }

        private void NavigationViewItem_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            IsSelected = true;
        }

        public static readonly StyledProperty<string> IconPathProperty =
            AvaloniaProperty.Register<NavigationViewItem, string>(nameof(IconPath));

        public string IconPath
        {
            get => GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<NavigationViewItem, string>(nameof(Text));

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static new readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<NavigationViewItem, IBrush>(nameof(Foreground));

        public new IBrush Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly StyledProperty<string> LocalizeKeyProperty =
            AvaloniaProperty.Register<NavigationViewItem, string>(nameof(LocalizeKey));

        public string LocalizeKey
        {
            get => GetValue(LocalizeKeyProperty);
            set => SetValue(LocalizeKeyProperty, value);
        }

        public static readonly StyledProperty<UserControl> FrameContentProperty =
            AvaloniaProperty.Register<NavigationViewItem, UserControl>(nameof(FrameContent));

        public UserControl FrameContent
        {
            get => GetValue(FrameContentProperty);
            set => SetValue(FrameContentProperty, value);
        }

        public static readonly StyledProperty<bool> IsSelectedProperty =
           AvaloniaProperty.Register<NavigationViewItem, bool>(nameof(IsSelected));

        public bool IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly StyledProperty<IBrush> SelectedMarkColorProperty =
            AvaloniaProperty.Register<NavigationViewItem, IBrush>(nameof(SelectedMarkColor));

        public IBrush SelectedMarkColor
        {
            get => GetValue(SelectedMarkColorProperty);
            set => SetValue(SelectedMarkColorProperty, value);
        }

        public bool Match(object data)
        {
            return true;
        }

        public IControl Build(object param)
        {
            return this;
        }
    }
}
