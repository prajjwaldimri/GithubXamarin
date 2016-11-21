using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using GithubUWP.ViewModels;
using Octokit;

namespace GithubUWP.Services.Converters
{
    public class EventToSentenceConverter : DependencyObject,IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
       {
            var currentActivity = value as Activity;
            var eventName = currentActivity.Type;
            var eventSentence = "";
            switch (eventName)
            {
                case "IssuesEvent":
                    var issueEventPayload = currentActivity.Payload as IssueEventPayload;
                    eventSentence = issueEventPayload.Action +" an issue at ";
                    break;
                case "ForkEvent":
                    eventSentence = " forked ";
                    break;
                case "WatchEvent":
                    eventSentence = " starred ";
                    break;
                case "PushEvent":
                    eventSentence = " pushed to ";
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
