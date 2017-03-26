using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace GithubXamarin.UWP.Resources.Converters
{
    public class ColorSwapper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;
            var color = (value as SolidColorBrush).Color;
            var a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
            return a > 0.5 ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
