using System;
using System.Globalization;
using Amareat.Helpers;
using Xamarin.Forms;

namespace Amareat.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string resource = string.Empty;

            if (value != null)
            {
                switch (value.ToString())
                {
                    case "0":
                        resource = Resources.Active;
                        break;
                    case "1":
                        resource = Resources.Inactive;
                        break;
                }
            }

            return resource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
