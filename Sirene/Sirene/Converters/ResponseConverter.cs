using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sirene.Converters
{
    public class ResponseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string response = value as string;

            if (response == Constants.SafeResponse)
            {
                return Constants.SafeIcon;
            }
            else if (response == Constants.NeedHelpResponse)
            {
                return Constants.NeedHelpIcon;
            }
            else
            {
                return response;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}