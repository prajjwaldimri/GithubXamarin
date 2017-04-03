using System;
using System.Globalization;
using Android.Graphics;
using MvvmCross.Platform.Converters;

namespace GithubXamarin.Droid.Converters
{
    public class HexToColorConverter : MvxValueConverter<string, Color>
    {
        protected override Color Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            value =  $"#{value}";
            var color = Color.ParseColor(value);
            return color;
        }
    }
}