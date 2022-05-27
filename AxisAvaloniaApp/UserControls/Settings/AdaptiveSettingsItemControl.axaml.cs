using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AxisAvaloniaApp.UserControls.Settings
{
    public partial class AdaptiveSettingsItemControl : UserControl
    {
        public static readonly DirectProperty<AdaptiveSettingsItemControl, string> TitleKeyProperty =
        AvaloniaProperty.RegisterDirect<AdaptiveSettingsItemControl, string>(
        nameof(TitleKey),
        o => o.TitleKey,
        (o, v) => o.TitleKey = v);

        private string _TitleKey = "UnknownKey";

        public string TitleKey
        {
            get { return _TitleKey; }
            set { SetAndRaise(TitleKeyProperty, ref _TitleKey, value); }
        }

        public static readonly DirectProperty<AdaptiveSettingsItemControl, double> TitleMinWidthProperty =
               AvaloniaProperty.RegisterDirect<AdaptiveSettingsItemControl, double>(
               nameof(TitleMinWidth),
               o => o.TitleMinWidth,
               (o, v) => o.TitleMinWidth = v);

        private double _TitleMinWidth = 100;

        public double TitleMinWidth
        {
            get { return _TitleMinWidth; }
            set { SetAndRaise(TitleMinWidthProperty, ref _TitleMinWidth, value); }
        }

        public static readonly DirectProperty<AdaptiveSettingsItemControl, double> InputMinWidthProperty =
               AvaloniaProperty.RegisterDirect<AdaptiveSettingsItemControl, double>(
               nameof(InputMinWidth),
               o => o.InputMinWidth,
               (o, v) => o.InputMinWidth = v);

        private double _InputMinWidth = 255;

        public double InputMinWidth
        {
            get { return _InputMinWidth; }
            set { SetAndRaise(InputMinWidthProperty, ref _InputMinWidth, value); }
        }



        public static readonly DirectProperty<AdaptiveSettingsItemControl, Control> InputProperty =
                         AvaloniaProperty.RegisterDirect<AdaptiveSettingsItemControl, Control>(
        nameof(Input),
        o => o.Input,
        (o, v) => o.Input = v);

        private Control _input = new TextBox();

        public Control Input
        {
            get { return _input; }
            set { SetAndRaise(InputProperty, ref _input, value); }
        }

        private bool isVertical = false;
        public bool IsVertical
        {
            get { return isVertical; }
            set { isVertical = value; }
        }

        public AdaptiveSettingsItemControl()
        {
            InitializeComponent();
            this.MinWidth = InputMinWidth > TitleMinWidth ? InputMinWidth : TitleMinWidth;
            this.PropertyChanged += AdaptiveSettingsItemControl_PropertyChanged;
        }

        

        private void AdaptiveSettingsItemControl_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {

            if (e.Property.Name == "Bounds" && this.Content != null &&  ((Grid)this.Content).Children[0] != null)
            {
                IsVertical = this.Bounds.Width - ((Border)((Grid)this.Content).Children[0]).Child.Bounds.Width < InputMinWidth;
                if (IsVertical)
                {
                    Grid.SetColumnSpan(((Grid)this.Content).Children[0] as Control, 2);
                    Grid.SetColumn(((Grid)this.Content).Children[0] as Control, 0);
                    Grid.SetRow(((Grid)this.Content).Children[0] as Control, 0);
                    Grid.SetRowSpan(((Grid)this.Content).Children[0] as Control, 1);

                    Grid.SetColumnSpan(((Grid)this.Content).Children[1] as Control, 2);
                    Grid.SetColumn(((Grid)this.Content).Children[1] as Control, 0);
                    Grid.SetRow(((Grid)this.Content).Children[1] as Control, 1);
                    Grid.SetRowSpan(((Grid)this.Content).Children[1] as Control, 1);

                    ((Control)((Border)((Grid)this.Content).Children[0]).Child).HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                    ((TextBlock)((Border)((Grid)this.Content).Children[0]).Child).TextAlignment = Avalonia.Media.TextAlignment.Center;
                } 
                else                
                {
                    Grid.SetColumnSpan(((Grid)this.Content).Children[0] as Control, 1);
                    Grid.SetColumn(((Grid)this.Content).Children[0] as Control, 0);
                    Grid.SetRow(((Grid)this.Content).Children[0] as Control, 0);
                    Grid.SetRowSpan(((Grid)this.Content).Children[0] as Control, 2);

                    Grid.SetColumnSpan(((Grid)this.Content).Children[1] as Control, 1);
                    Grid.SetColumn(((Grid)this.Content).Children[1] as Control, 1);
                    Grid.SetRow(((Grid)this.Content).Children[1] as Control, 0);
                    Grid.SetRowSpan(((Grid)this.Content).Children[1] as Control, 2);

                    ((Control)((Border)((Grid)this.Content).Children[0]).Child).HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
                    ((TextBlock)((Border)((Grid)this.Content).Children[0]).Child).TextAlignment = Avalonia.Media.TextAlignment.Left;
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
