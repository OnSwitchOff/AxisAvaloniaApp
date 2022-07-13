using Avalonia.Data.Converters;
using AxisAvaloniaApp.Enums;
using System;
using System.Globalization;

namespace AxisAvaloniaApp.Converters
{
    public class EPeriodsToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter == null)
                {
                    return true;
                }

                EPeriods val, par;
                if (Enum.TryParse<EPeriods>(value.ToString(), out val))
                {
                    if (Enum.TryParse<EPeriods>(parameter.ToString(), out par))
                    {
                        return val == par;
                    }
                }
            }

            throw new NotImplementedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
