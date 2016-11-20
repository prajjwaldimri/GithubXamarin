using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GithubUWP.Services.Converters
{
    public class EventToSentenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var eventName = value.ToString();
            var eventSentence = "";
            switch (eventName)
            {
                case "IssuesEvent":
                    eventSentence = "opened an issue at";
                    break;
                case "ForkEvent":
                    eventSentence = "forked";
                    break;
                case "WatchEvent":
                    eventSentence = "starred";
                    break;
                case "PushEvent":
                    eventSentence = "pushed to";
                    break;
                default:
                    eventSentence = "";
                    break;
            }
            return eventSentence;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
