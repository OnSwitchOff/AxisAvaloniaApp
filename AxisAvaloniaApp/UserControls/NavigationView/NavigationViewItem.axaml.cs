using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace AxisAvaloniaApp.UserControls.NavigationView
{
    public class NavigationViewItem : TemplatedControl, ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewItem"/> class.
        /// </summary>
        public NavigationViewItem()
        {
            SelectedMarkColor = Brushes.White;
            Foreground = Brushes.White;
            IsSelected = false;

            AddHandler(PointerPressedEvent, NavigationViewItem_PointerPressed, Avalonia.Interactivity.RoutingStrategies.Tunnel);
        }

        /// <summary>
        /// Update "IsSelected" property if NavigationViewItem is pressed.
        /// </summary>
        /// <param name="sender">NavigationViewItem.</param>
        /// <param name="e">Event args.</param>
        /// <date>17.05.2022.</date>
        private void NavigationViewItem_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            IsSelected = true;
        }

        public static readonly StyledProperty<string> IconPathProperty =
            AvaloniaProperty.Register<NavigationViewItem, string>(nameof(IconPath));

        /// <summary>
        /// Path to icon.
        /// </summary>
        /// <date>17.05.2022.</date>
        public string IconPath
        {
            get => GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<NavigationViewItem, string>(nameof(Text));

        /// <summary>
        /// Text that shown by user.
        /// </summary>
        /// <date>17.05.2022.</date>
        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static new readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<NavigationViewItem, IBrush>(nameof(Foreground));

        /// <summary>
        /// Foreground of text.
        /// </summary>
        /// <date>17.05.2022.</date>
        public new IBrush Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly StyledProperty<string> LocalizeKeyProperty =
            AvaloniaProperty.Register<NavigationViewItem, string>(nameof(LocalizeKey));

        /// <summary>
        /// Key to get text in according to using language. 
        /// </summary>
        /// <date>17.05.2022.</date>
        public string LocalizeKey
        {
            get => GetValue(LocalizeKeyProperty);
            set => SetValue(LocalizeKeyProperty, value);
        }

        public static readonly StyledProperty<bool> IsSelectedProperty =
           AvaloniaProperty.Register<NavigationViewItem, bool>(nameof(IsSelected), false);

        /// <summary>
        /// Flag indicating whether the NavigationViewItem is selected by user.
        /// </summary>
        /// <date>17.05.2022.</date>
        public bool IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly StyledProperty<IBrush> SelectedMarkColorProperty =
            AvaloniaProperty.Register<NavigationViewItem, IBrush>(nameof(SelectedMarkColor));

        /// <summary>
        /// Color of mark to show the NavigationViewItem is selected.
        /// </summary>
        /// <date>17.05.2022.</date>
        public IBrush SelectedMarkColor
        {
            get => GetValue(SelectedMarkColorProperty);
            set => SetValue(SelectedMarkColorProperty, value);
        }
    }
}
