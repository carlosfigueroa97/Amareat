using System;
using System.Globalization;
using Amareat.Helpers;
using Xamarin.Forms;

namespace Amareat.Converters
{
    public class ChangeColorPowerIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool)value)
            {
                return Colors.OrangeColor;
            }

            return Colors.SecondSolidGrayColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Colors.SecondSolidGrayColor;
        }
    }
}
