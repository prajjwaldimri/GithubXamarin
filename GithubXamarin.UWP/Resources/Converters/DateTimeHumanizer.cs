using System;
using Windows.UI.Xaml.Data;
using Humanizer;

namespace GithubXamarin.UWP.Resources.Converters
{
    public class DateTimeHumanizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((DateTime?) value)?.Humanize();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
