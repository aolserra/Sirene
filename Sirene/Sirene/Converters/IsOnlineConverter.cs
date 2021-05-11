using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sirene.Converters
{
    public class IsOnlineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsOnline = (bool)value;

            if (IsOnline)
            {
                return Constants.OnlineIcon;
            }
            else
            {
                return Constants.OfflineIcon;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}