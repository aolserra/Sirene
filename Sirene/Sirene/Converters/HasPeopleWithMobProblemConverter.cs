using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sirene.Converters
{
    public class HasPeopleWithMobProblemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasPeopleWithMobProblem = (bool)value;

            if (hasPeopleWithMobProblem)
            {
                return Constants.HasPeopleWithMobProblemIcon;
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
