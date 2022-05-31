using Avalonia.Data.Converters;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        private readonly ISettingsService settingsService;

        public DoubleToStringConverter()
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
        }


        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                EDoubleTypes doubleType;
                if (Enum.TryParse<EDoubleTypes>(parameter.ToString(), out doubleType))
                {
                    double parsedValue;
                    if (value == null)
                    {
                        throw new Exception("Value is not initialized!");
                    }

                    if (double.TryParse(value.ToString(), out parsedValue))
                    {
                        switch (doubleType)
                        {
                            case EDoubleTypes.Qty:
                                return parsedValue.ToString(settingsService.QtyFormat);
                            case EDoubleTypes.Price:
                                return parsedValue.ToString(settingsService.PriceFormat);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(); 
                    }
                }
            }
            

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            double parsedValue;
            if (double.TryParse(value.ToString(), out parsedValue))
            {
                return parsedValue;
            }

            throw new ArgumentException();
        }
    }
}
