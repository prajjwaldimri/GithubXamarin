using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Octokit;

namespace GithubXamarin.Core.Converters
{
    /// <summary>
    /// Takes an event from the binding and return proper description for it.
    /// </summary>
    public class EventToStringConverter : MvxValueConverter<Activity,string>
    {
        protected override string Convert(Activity value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            //https://developer.github.com/v3/activity/events/types/
            string eventString = null;
            switch (value.Type)
            {
                case "CommitCommentEvent":
                    eventString = $"commented on";
                    break;
                case "CreateEvent":
                    eventString = $"created";
                    break;
                case "DeleteEvent":
                    eventString = $"deleted";
                    break;
                case "ForkEvent":
                    eventString = $"forked";
                    break;
                case "GollumEvent":
                    eventString = "created/edited a wiki page on";
                    break;
                case "IssuesEvent":
                    eventString = $"edited an issue {value.Payload}";
                    break;
                case "LabelEvent":
                    eventString = $"";
                    break;
                case "MilestoneEvent":
                    eventString = $"";
                    break;
                case "PullRequestEvent":
                    eventString = $"";
                    break;
                case "PushEvent":
                    eventString = $"";
                    break;
                case "ReleaseEvent":
                    eventString = $"";
                    break;
                case "RepositoryEvent":
                    eventString = $"";
                    break;
                case "StatusEvent":
                    eventString = $"";
                    break;
                case "WatchEvent":
                    eventString = $"";
                    break;
                default:
                    return string.Empty;
            }
            //TODO: Replace
            return string.Empty;
        }
    }
}
