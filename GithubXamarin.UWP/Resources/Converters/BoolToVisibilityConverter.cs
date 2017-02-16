using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var boolValue = (bool) value;
            switch (boolValue)
            {
                case true:
                    return Visibility.Visible;
                    break;
                default:
                    return Visibility.Collapsed;
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var visibilityValue = (Visibility) value;
            switch (visibilityValue)
            {
                case Visibility.Visible:
                    return true;
                    break;
                case Visibility.Collapsed:
                    return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
