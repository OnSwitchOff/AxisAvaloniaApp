using Avalonia.Data.Converters;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AxisAvaloniaApp.Converters
{
    public class ENomenclaturesToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                ENomenclatures val, par;
                if (Enum.TryParse<ENomenclatures>(value.ToString(), out val))
                {
                    if (Enum.TryParse<ENomenclatures>(parameter.ToString(), out par))
                    {
                        return val == par;
                    }
                    else
                    {
                        return parameter.ToString().Contains(val.ToString());
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
