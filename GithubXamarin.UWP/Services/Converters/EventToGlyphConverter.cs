using System;
using Windows.UI.Xaml.Data;

namespace GithubXamarin.UWP.Services.Converters
{
    public class EventToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var eventName = value.ToString();
            var eventGlyph = "";
            switch (eventName)
            {
                case "IssuesEvent":
                    eventGlyph = "\uf44d";
                    break;
                case "ForkEvent":
                    eventGlyph = "\uf2c0";
                    break;
                case "WatchEvent":
                    eventGlyph = "\uf133";
                    break;
                case "PushEvent":
                    eventGlyph = "\uf255";
                    break;
                default:
                    eventGlyph = "\uf279";
                    break;
            }
            return eventGlyph;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
