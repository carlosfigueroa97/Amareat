using System;
using System.Globalization;
using Amareat.Helpers;
using Xamarin.Forms;

namespace Amareat.Converters
{
    public class StatusOnOffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string resource = string.Empty;

            if (value != null)
            {
                switch (value)
                {
                    case true:
                        resource = Resources.On;
                        break;
                    case false:
                        resource = Resources.Off;
                        break;
                }
            }

            return resource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
