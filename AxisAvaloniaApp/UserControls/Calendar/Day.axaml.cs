using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AxisAvaloniaApp.UserControls.Extensions;
using System;

namespace AxisAvaloniaApp.UserControls.MyCalendar
{
    public partial class Day : UserControl
    {
        Border outerBorder;
        Border innerBorder;
        AxisTextBlock axisTextBlock;

        public static readonly DirectProperty<Day, DateTime> DateProperty =
              AvaloniaProperty.RegisterDirect<Day, DateTime>(
              nameof(Date),
              o => o.Date,
              (o, v) => o.Date = v);
        private DateTime _Date = DateTime.Now;
        public DateTime Date
        {
            get { return _Date; }
            set
            { 
                SetAndRaise(DateProperty, ref _Date, value);
                axisTextBlock.Text = Date.Day.ToString();
            }
        }

        public static readonly DirectProperty<Day, bool> IsSelectedProperty =
              AvaloniaProperty.RegisterDirect<Day, bool>(
              nameof(IsSelected),
              o => o.IsSelected,
              (o, v) => o.IsSelected = v);
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set 
            { 
                SetAndRaise(IsSelectedProperty, ref _IsSelected, value);
                Rerender();
            }
        }

        public static readonly DirectProperty<Day, bool> IsCurrentProperty =
            AvaloniaProperty.RegisterDirect<Day, bool>(
            nameof(IsCurrent),
            o => o.IsCurrent,
            (o, v) => o.IsCurrent = v);
        private bool _IsCurrent = false;
        public bool IsCurrent
        {
            get { return _IsCurrent; }
            set
            {
                SetAndRaise(IsCurrentProperty, ref _IsCurrent, value);
                Rerender();
            }
        }

        public static readonly DirectProperty<Day, bool> IsCurrentMonthProperty =
            AvaloniaProperty.RegisterDirect<Day, bool>(
            nameof(IsCurrentMonth),
            o => o.IsCurrentMonth,
            (o, v) => o.IsCurrentMonth = v);
        private bool _IsCurrentMonth = false;
        public bool IsCurrentMonth
        {
            get { return _IsCurrentMonth; }
            set
            {
                SetAndRaise(IsCurrentMonthProperty, ref _IsCurrentMonth, value);
                Rerender();
            }
        }

        public Day()
        {
            InitializeComponent();
            outerBorder = this.FindControl<Border>("OuterBorder");
            innerBorder = this.FindControl<Border>("InnerBorder");
            axisTextBlock = this.FindControl<AxisTextBlock>("AxisTextBlock");

            outerBorder.PointerEnter += Border_PointerEnter;
            outerBorder.PointerLeave += Border_PointerLeave;
        }

        private void Border_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            IsSelected = IsSelected;
        }


        private void Border_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {            
            outerBorder.BorderBrush = IsCurrent? Brushes.SteelBlue : new  SolidColorBrush(Color.FromRgb(150,150,150));
            outerBorder.Background = Brushes.Transparent;
        }

        private void Rerender()
        {
            if (IsSelected && IsCurrent)
            {
                outerBorder.BorderBrush = Brushes.SteelBlue;
                outerBorder.Background = Brushes.Transparent;
                innerBorder.Background = Brushes.SteelBlue;
                axisTextBlock.Foreground = Brushes.White;
            }
            else if (IsSelected)
            {
                outerBorder.BorderBrush = Brushes.SteelBlue;
                outerBorder.Background = Brushes.Transparent;
                innerBorder.Background = Brushes.Transparent;
                axisTextBlock.Foreground = Brushes.Black;
            }
            else if (IsCurrent)
            {
                outerBorder.BorderBrush = Brushes.SteelBlue;
                outerBorder.Background = Brushes.SteelBlue;
                innerBorder.Background = Brushes.SteelBlue;
                axisTextBlock.Foreground = Brushes.White;
            }
            else
            {
                outerBorder.BorderBrush = Brushes.Transparent;
                outerBorder.Background = Brushes.Transparent;
                innerBorder.Background = Brushes.Transparent;
                axisTextBlock.Foreground = Brushes.Black;
            }

            if (!IsCurrentMonth && !IsSelected)
            {
                axisTextBlock.Foreground = Brushes.Gray;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
