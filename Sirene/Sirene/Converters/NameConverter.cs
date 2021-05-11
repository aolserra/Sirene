using System;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sirene.Converters
{
    public class NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string nome = value as string;
            string[] palavras = nome.ToLower().FirstCharWordsToUpper().Split(' ');
            return palavras[0] + " " + palavras[palavras.Length - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}