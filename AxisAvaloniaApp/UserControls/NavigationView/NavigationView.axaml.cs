using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using AxisAvaloniaApp.UserControls.Models;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.UserControls.NavigationView
{
    public class NavigationView : TemplatedControl
    {
        private void fgfg(object? sender, PointerPressedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public static readonly StyledProperty<bool> IsMenuExpandedProperty =
            AvaloniaProperty.Register<NavigationView, bool>(nameof(IsMenuExpanded), true);

        public bool IsMenuExpanded
        {
            get => GetValue(IsMenuExpandedProperty);
            set => SetValue(IsMenuExpandedProperty, value);
        }

        public static readonly StyledProperty<double> CollapsedMenuWidthProperty =
            AvaloniaProperty.Register<NavigationView, double>(nameof(CollapsedMenuWidth), 40);

        public double CollapsedMenuWidth
        {
            get => GetValue(CollapsedMenuWidthProperty);
            set => SetValue(CollapsedMenuWidthProperty, value);
        }

        public static readonly StyledProperty<double> ExpandedMenuWidthProperty =
            AvaloniaProperty.Register<NavigationView, double>(nameof(ExpandedMenuWidth), 200);

        public double ExpandedMenuWidth
        {
            get => GetValue(ExpandedMenuWidthProperty);
            set => SetValue(ExpandedMenuWidthProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<MainMenuItemModel>> MainMenuItemsProperty =
            AvaloniaProperty.Register<NavigationView, ObservableCollection<MainMenuItemModel>>(nameof(MainMenuItems));

        public ObservableCollection<MainMenuItemModel> MainMenuItems
        {
            get => GetValue(MainMenuItemsProperty);
            set => SetValue(MainMenuItemsProperty, value);
        }

        public static readonly StyledProperty<MainMenuItemModel> SelectedItemProperty =
            AvaloniaProperty.Register<NavigationView, MainMenuItemModel>(nameof(SelectedItem));

        public MainMenuItemModel SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set
            {
                if (SelectedItem != null)
                {
                    SelectedItem.IsSelected = false;
                }
                SetValue(SelectedItemProperty, value);
            }
        }

        public static readonly StyledProperty<Control> FrameProperty =
            AvaloniaProperty.Register<NavigationView, Control>(nameof(Frame));

        public Control Frame
        {
            get => GetValue(FrameProperty);
            set => SetValue(FrameProperty, value);
        }

        private void MainMenuItem_PointerEnter(object? sender, PointerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void MainMenuItem_PointerLeave(object? sender, PointerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //public static readonly StyledProperty<bool> IsCheckedProperty =
        //    AvaloniaProperty.Register<NavigationView, bool>("IsChecked");

        //public bool IsChecked
        //{
        //    get => GetValue(IsCheckedProperty);
        //    set => SetValue(IsCheckedProperty, value);
        //}

        public NavigationView()
        {
            //TextBlock
            //AddHandler(PointerPressedEvent, Handler, RoutingStrategies.Tunnel);
        }

        //private void Handler(object? sender, PointerPressedEventArgs e)
        //{
        //    IsChecked = !IsChecked;
        //}
    }
}
