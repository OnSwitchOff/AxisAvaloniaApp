using Avalonia.Data.Converters;
using AxisAvaloniaApp.Enums;
using System;
using System.Globalization;

namespace AxisAvaloniaApp.Converters
{
    public class EPrintingModesToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                EPrintingModes val, par;
                if (Enum.TryParse<EPrintingModes>(value.ToString(), out val))
                {
                    if (Enum.TryParse<EPrintingModes>(parameter.ToString(), out par))
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
