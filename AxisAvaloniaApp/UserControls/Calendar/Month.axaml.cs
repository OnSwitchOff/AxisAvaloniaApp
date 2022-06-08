using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AxisAvaloniaApp.UserControls.Extensions;
using System;

namespace AxisAvaloniaApp.UserControls.MyCalendar
{
    public partial class Month : UserControl
    {
        public static readonly DirectProperty<Month, DateTime> SelectedDateProperty =
               AvaloniaProperty.RegisterDirect<Month, DateTime>(
               nameof(SelectedDate),
               o => o.SelectedDate,
               (o, v) => o.SelectedDate = v);
        private DateTime _SelectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { SetAndRaise(SelectedDateProperty, ref _SelectedDate, value); }
        }

        public static readonly DirectProperty<Month, DateTime> FirstDateProperty =
               AvaloniaProperty.RegisterDirect<Month, DateTime>(
               nameof(FirstDate),
               o => o.FirstDate,
               (o, v) => o.FirstDate = v);
        private DateTime _FirstDate = DateTime.Now;
        public DateTime FirstDate
        {
            get { return _FirstDate; }
            set 
            {
                _FirstDate = value;
                RenderDays();
                string tmp = FirstDate.ToString("MMMM yyyy");
                currentMonthTextBlock.Text = char.ToUpper(tmp[0]) + tmp.Substring(1);
            }
        }

        private Day selectedDay;
        private Grid daysGrid;
        private TextBlock currentMonthTextBlock;
        public Month()
        {
            InitializeComponent();
            daysGrid = this.FindControl<Grid>("DaysGrid");
            currentMonthTextBlock = this.FindControl<TextBlock>("CurrentMonthTextBlock");
            FirstDate = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);

        }

       

        private void RenderDays()
        {
            while(daysGrid.Children.Count > 7)
            {
                daysGrid.Children.Remove(daysGrid.Children[daysGrid.Children.Count - 1]);
            }

            int offset = -((int)FirstDate.DayOfWeek + 7 - (int)DayOfWeek.Monday) % 7;

            for (int i = 1; i < 7; i++)
            {
                for (int j = 0; j < 7; j++, offset++)
                {     
                    Day day = new Day();
                    day.Date = FirstDate.AddDays(offset);
                    Grid.SetRow(day, i);
                    Grid.SetColumn(day, j);
                    day.IsSelected = SelectedDate.ToString("dd.MM.yyyy") == day.Date.ToString("dd.MM.yyyy");
                    day.IsCurrent = DateTime.Now.ToString("dd.MM.yyyy") == day.Date.ToString("dd.MM.yyyy");
                    day.IsCurrentMonth = day.Date.Month == FirstDate.Month;
                    if (day.IsSelected)
                    {
                        selectedDay = day;
                    }
                    daysGrid.Children.Add(day);
                    day.PointerReleased += Day_PointerReleased;
                }
            }            
        }

        private void Day_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            if (selectedDay != null)
            {
                selectedDay.IsSelected = false;
            }
            selectedDay = (Day)sender;
            selectedDay.IsSelected = true;
            SelectedDate = selectedDay.Date;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnPreviousClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            FirstDate = FirstDate.AddMonths(-1);
        }

        private void OnNextClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            FirstDate = FirstDate.AddMonths(1);
        }
    }
}
