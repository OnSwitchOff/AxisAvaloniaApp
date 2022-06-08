using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace AxisAvaloniaApp.UserControls.MyCalendar
{
    public partial class MyDatePicker : UserControl
    {
        public static readonly DirectProperty<MyDatePicker, bool> IsExpandedProperty =
             AvaloniaProperty.RegisterDirect<MyDatePicker, bool>(
             nameof(IsExpanded),
             o => o.IsExpanded,
             (o, v) => o.IsExpanded = v);
        private bool _IsExpanded = false;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                SetAndRaise(IsExpandedProperty, ref _IsExpanded, value);
                if (monthView != null && IsExpanded)
                {
                    monthView.FirstDate = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
                }
            }
        }

        public static readonly DirectProperty<MyDatePicker, DateTime> SelectedDateProperty =
              AvaloniaProperty.RegisterDirect<MyDatePicker, DateTime>(
              nameof(SelectedDate),
              o => o.SelectedDate,
              (o, v) => o.SelectedDate = v);
        private DateTime _SelectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set
            { 
                SetAndRaise(SelectedDateProperty, ref _SelectedDate, value);
                SelectedDateString = SelectedDate.ToString("dd MMMM yyyy");
                IsExpanded = false;
            }
        }

        public static readonly DirectProperty<MyDatePicker, string> SelectedDateStringProperty =
             AvaloniaProperty.RegisterDirect<MyDatePicker, string>(
             nameof(SelectedDateString),
             o => o.SelectedDateString,
             (o, v) => o.SelectedDateString = v);
        private string _SelectedDateString = DateTime.Now.ToString("dd MMMM yyyy");
        public string SelectedDateString
        {
            get { return _SelectedDateString; }
            set
            {
                SetAndRaise(SelectedDateStringProperty, ref _SelectedDateString, value);
                IsExpanded = false;
            }
        }

        Month monthView;

        public MyDatePicker()
        {
            InitializeComponent();
            monthView = this.FindControl<Month>("MonthView");
        }

        private void PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            IsExpanded = false;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }
    }
}
