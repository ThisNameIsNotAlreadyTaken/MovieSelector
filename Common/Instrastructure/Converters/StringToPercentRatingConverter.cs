using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Common.Instrastructure.Converters
{
    public class StringToPercentRatingConverter : IValueConverter
    {
        private static readonly Regex DoubleRegex = new Regex(@"^-?\d+(?:\.\d+)?");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            double? result = null;

            if (s != null)
            {
                var match = DoubleRegex.Match(s);

                if (match.Success)
                {
                    var matchedValue = double.Parse(match.Value, CultureInfo.InvariantCulture);
                    result = matchedValue / (matchedValue > 10 ? 100 : 10);
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
