
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GithubXamarin.UWP.Resources.Converters
{
    class ReverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var boolValue = (bool)value;
            switch (boolValue)
            {
                case true:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
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
                    return false;
                case Visibility.Collapsed:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
