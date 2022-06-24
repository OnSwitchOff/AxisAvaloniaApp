using Avalonia.Data.Converters;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Services.Settings;
using Splat;
using System;
using System.Globalization;

namespace AxisAvaloniaApp.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        private readonly ISettingsService settingsService;

        public DoubleToStringConverter()
        {
            settingsService = Locator.Current.GetService<ISettingsService>();
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
                            case EDoubleTypes.F0_Percent:
                                return ((double)value).ToString("F0") + "%";
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
            if (parameter != null)
            {
                EDoubleTypes doubleType;
                if (Enum.TryParse<EDoubleTypes>(parameter.ToString(), out doubleType))
                {
                    if (value == null)
                    {
                        throw new Exception("Value is not initialized!");
                    }

                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        return 0;
                    }
                    
                    char separator = '.';
                    switch (doubleType)
                    {
                        case EDoubleTypes.Qty:
                            separator = settingsService.Culture.NumberFormat.NumberDecimalSeparator[0];
                            break;
                        case EDoubleTypes.Price:
                            separator = settingsService.Culture.NumberFormat.CurrencyDecimalSeparator[0];
                            break;
                        case EDoubleTypes.F0_Percent:
                            separator = settingsService.Culture.NumberFormat.PercentDecimalSeparator[0];
                            break;
                    }

                    if (double.TryParse(value.ToString().Replace(',', separator).Replace('.', separator).TrimEnd(separator), out parsedValue))
                    {
                        return parsedValue;
                    }
                }
            }

            throw new ArgumentException();
        }
    }
}
