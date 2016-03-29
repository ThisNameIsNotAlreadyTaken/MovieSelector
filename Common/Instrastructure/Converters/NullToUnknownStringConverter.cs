using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Instrastructure.Converters
{
    public class NullToUnknownStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
