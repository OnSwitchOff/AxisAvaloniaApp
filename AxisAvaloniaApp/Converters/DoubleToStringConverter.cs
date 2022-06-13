﻿using Avalonia.Data.Converters;
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
            if (double.TryParse(value.ToString(), out parsedValue))
            {
                return parsedValue;
            }

            throw new ArgumentException();
        }
    }
}
