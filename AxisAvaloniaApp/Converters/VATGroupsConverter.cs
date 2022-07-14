using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AxisAvaloniaApp.Converters
{
    public class VATGroupsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && value is Models.VATGroupModel vATGroup)
            {
                if (parameter != null && 
                    parameter is Avalonia.Data.Binding binding && 
                    binding.NameScope != null && 
                    binding.NameScope.TryGetTarget(out var nameScope))
                {
                    object control = nameScope.Find("comboBoxVATGroups");
                    if (control != null && control is Avalonia.Controls.ComboBox comboBox)
                    {
                        foreach (Models.VATGroupModel model in comboBox.Items)
                        {
                            if (model.Id == vATGroup.Id)
                            {
                                return model;
                            }
                        }
                    }
                }
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
