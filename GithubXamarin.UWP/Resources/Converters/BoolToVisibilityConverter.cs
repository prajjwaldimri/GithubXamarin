using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GithubXamarin.UWP.Resources.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var boolValue = (bool)value;
            switch (boolValue)
            {
                case true:
                    return Visibility.Visible;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var visibilityValue = (Visibility)value;
            switch (visibilityValue)
            {
                case Visibility.Visible:
                    return true;
                case Visibility.Collapsed:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
