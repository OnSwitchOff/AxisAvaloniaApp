using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AxisAvaloniaApp.Converters
{
    public class EItemTypesToComboBoxItemModelConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && parameter != null && parameter is Avalonia.Data.Binding binding)
            {
                binding.NameScope.TryGetTarget(out var nameScope);
                Avalonia.Controls.ComboBox control = nameScope.Find("comboBoxItemsTypes") as Avalonia.Controls.ComboBox;
                if (control != null)
                {
                    Microinvest.CommonLibrary.Enums.EItemTypes type = Enum.Parse<Microinvest.CommonLibrary.Enums.EItemTypes>(value.ToString());
                    foreach (UserControls.Models.ComboBoxItemModel item in control.Items)
                    {
                        if (item.Value is Microinvest.CommonLibrary.Enums.EItemTypes itemType && itemType == type)
                        {
                            return item;
                        }
                    }
                }
            }

            throw new ArgumentException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && value is UserControls.Models.ComboBoxItemModel item)
            {
                return item.Value;
            }

            throw new ArgumentException();
        }
    }
}
