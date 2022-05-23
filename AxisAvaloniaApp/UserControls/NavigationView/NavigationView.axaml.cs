using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.UserControls.NavigationView
{
    public class NavigationView :  TemplatedControl
    {
        private const double collapsedWidth = 65;

        public override void EndInit()
        {
            base.EndInit();
        }

        public static readonly StyledProperty<bool> IsCollapsedProperty =
            AvaloniaProperty.Register<NavigationView, bool>(nameof(IsCollapsed), true);

        /// <summary>
        /// Flag indicating whether the panel is collapsed.
        /// </summary>
        /// <date>18.05.2022.</date>
        public bool IsCollapsed
        {
            get => GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }

        public new static readonly StyledProperty<double> WidthProperty =
            AvaloniaProperty.Register<NavigationView, double>(nameof(Width), double.NaN);

        /// <summary>
        /// Width of the expanded panel.
        /// </summary>
        /// <date>18.05.2022.</date>
        public new double Width
        {
            get => GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public new static readonly StyledProperty<IBrush> ForegroundProperty =
           AvaloniaProperty.Register<NavigationView, IBrush>(nameof(Foreground));

        /// <summary>
        /// Color of text.
        /// </summary>
        /// <date>18.05.2022.</date>
        public new IBrush Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> MarkColorProperty =
            AvaloniaProperty.Register<NavigationView, IBrush>(nameof(MarkColor));

        /// <summary>
        /// Color of mark to select item.
        /// </summary>
        /// <date>18.05.2022.</date>
        public IBrush MarkColor
        {
            get => GetValue(MarkColorProperty);
            set => SetValue(MarkColorProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<INavigationViewItem>> ItemsSourceProperty =
           AvaloniaProperty.Register<NavigationView, ObservableCollection<INavigationViewItem>>(nameof(ItemsSource));

        /// <summary>
        /// List with items.
        /// </summary>
        /// <date>19.05.2022.</date>
        public ObservableCollection<INavigationViewItem> ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly StyledProperty<INavigationViewItem> SelectedSourceItemProperty =
            AvaloniaProperty.Register<NavigationView, INavigationViewItem>(nameof(SelectedSourceItem));

        /// <summary>
        /// Selected item control.
        /// </summary>
        /// <date>18.05.2022.</date>
        public INavigationViewItem SelectedSourceItem
        {
            get => GetValue(SelectedSourceItemProperty);
            set
            {
                SetValue(SelectedSourceItemProperty, value);
            }
        }

        public static readonly StyledProperty<ObservableCollection<NavigationViewItem>> MenuItemsProperty =
           AvaloniaProperty.Register<NavigationView, ObservableCollection<NavigationViewItem>>(
               nameof(MenuItems),
               new ObservableCollection<NavigationViewItem>());

        /// <summary>
        /// List with items controls.
        /// </summary>
        /// <date>18.05.2022.</date>
        public ObservableCollection<NavigationViewItem> MenuItems
        {
            get => GetValue(MenuItemsProperty);
            set => SetValue(MenuItemsProperty, value);
        }

        public static readonly StyledProperty<NavigationViewItem> SelectedItemProperty =
            AvaloniaProperty.Register<NavigationView, NavigationViewItem>(nameof(SelectedItem));

        /// <summary>
        /// Selected item control.
        /// </summary>
        /// <date>18.05.2022.</date>
        public NavigationViewItem SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly StyledProperty<object> MenuContentProperty =
            AvaloniaProperty.Register<NavigationView, object>(nameof(MenuContent));

        /// <summary>
        /// Additional items of menu.
        /// </summary>
        /// <date>18.05.2022.</date>
        public object MenuContent
        {
            get => GetValue(MenuContentProperty);
            set => SetValue(MenuContentProperty, value);
        }

        public static readonly StyledProperty<IControl> ContentProperty =
           AvaloniaProperty.Register<NavigationView, IControl>(nameof(Content));

        /// <summary>
        /// Content of the main window.
        /// </summary>
        /// <date>18.05.2022.</date>
        public IControl Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Update dependents properties if main property was changed.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="change">History of changing of property.</param>
        /// <date>18.05.2022.</date>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            int index = -1;
            switch (change.Property.Name)
            {
                case nameof(IsCollapsed):
                    Width = IsCollapsed ? collapsedWidth : double.NaN;
                    break;
                case nameof(MenuItems):
                    if (change.NewValue.Value is Collection<NavigationViewItem> newItems)
                    {
                        foreach (NavigationViewItem item in newItems)
                        {
                            item.Foreground = Foreground;
                        }
                    }
                    break;
                case nameof(ItemsSource):
                    if (change.NewValue.Value is Collection<INavigationViewItem> newItemModels)
                    {
                        foreach (INavigationViewItem item in newItemModels)
                        {
                            MenuItems.Add(
                                new NavigationViewItem()
                                {
                                    IconPath = item.IconPath,
                                    LocalizeKey = item.LocalizeKey,
                                    Text = item.Text,
                                    IsSelected = item.IsSelected,
                                    Foreground = Foreground,
                                    SelectedMarkColor = MarkColor,
                                });
                        }
                    }
                    break;
                case nameof(SelectedItem):
                    if (change.OldValue.Value != null && change.NewValue.Value != null && change.OldValue.Value is NavigationViewItem viewItem)
                    {
                        viewItem.IsSelected = false;
                    }

                    SelectedItem.IsSelected = true;

                    if (ItemsSource != null && MenuItems.Contains(SelectedItem))
                    {
                        index = MenuItems.IndexOf(SelectedItem);
                        SelectedSourceItem = ItemsSource[index];
                    }
                    break;
                case nameof(SelectedSourceItem):
                    if (ItemsSource != null && ItemsSource.Contains(SelectedSourceItem))
                    {
                        index = ItemsSource.IndexOf(SelectedSourceItem);
                        SelectedItem = MenuItems[index];
                        SelectedItem.IsSelected = true;
                    }
                    
                    break;
            }

            base.OnPropertyChanged(change);
        }
    }
}
