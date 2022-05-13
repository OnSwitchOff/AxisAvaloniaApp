using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;

namespace AxisAvaloniaApp.UserControls
{
    public partial class MainMenuItem : UserControl
    {
        public MainMenuItem()
        {
            InitializeComponent();

            this.Height = 30;

            AddHandler(PointerPressedEvent, Handler, Avalonia.Interactivity.RoutingStrategies.Direct);
        }

        private void Handler(object? sender, PointerPressedEventArgs e)
        {
            IsSelected = !IsSelected;
        }

        public static readonly AttachedProperty<bool> IsSelectedProperty =
           AvaloniaProperty.RegisterAttached<MainMenuItem, Interactive, bool>(nameof(IsSelected));

        /// <summary>
        /// Gets or sets height of the MainMenuItem.
        /// </summary>
        /// <date>12.05.2022.</date>
        public bool IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set
            {
                SetValue(IsSelectedProperty, value);

                this.SelectedColor = value ? Colors.White : Colors.Transparent;
            }
        }

        public static readonly StyledProperty<Color> SelectedColorProperty =
           AvaloniaProperty.Register<MainMenuItem, Color>(nameof(SelectedColor));

        /// <summary>
        /// Gets or sets height of the MainMenuItem.
        /// </summary>
        /// <date>12.05.2022.</date>
        public Color SelectedColor
        {
            get => GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
