using System;
using System.Globalization;
using Humanizer;
using MvvmCross.Platform.Converters;

namespace GithubXamarin.Droid.Converters
{
    public class DateTimeHumanizer : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Humanize();
        }
    }
}