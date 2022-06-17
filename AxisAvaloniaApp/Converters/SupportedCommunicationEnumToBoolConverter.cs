using Avalonia.Data.Converters;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Converters
{
    public class SupportedCommunicationEnumToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                switch (value)
                {
                    case UserControls.Models.ComboBoxItemModel model:
                        SupportedCommunicationEnum communicationEnum;
                        if (Enum.TryParse<SupportedCommunicationEnum>(parameter.ToString(), out communicationEnum))
                        {
                            if (model.Value == null)
                            {
                                return false;
                            }

                            return model.Value.ToString().Equals(parameter.ToString());
                        }
                        
                        throw new NotImplementedException();
                    default:
                        return value.ToString().Equals(parameter.ToString());
                }
                
            }

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
