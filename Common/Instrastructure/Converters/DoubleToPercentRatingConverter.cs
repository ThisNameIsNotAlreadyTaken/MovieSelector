using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Instrastructure.Converters
{
   public class DoubleToPercentRatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = System.Convert.ToDouble(value);
            return doubleValue/10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
