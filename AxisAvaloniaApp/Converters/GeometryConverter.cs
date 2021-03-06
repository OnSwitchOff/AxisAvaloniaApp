using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace AxisAvaloniaApp.Converters
{
    public class GeometryConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string data && targetType.IsAssignableFrom(typeof(Geometry)))
			{
				return Geometry.Parse(data);
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
