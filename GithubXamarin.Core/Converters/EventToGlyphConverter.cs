using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Octokit;

namespace GithubXamarin.Core.Converters
{
    public class EventToGlyphConverter : MvxValueConverter<Activity, string>
    {
        protected override string Convert(Activity value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            //https://developer.github.com/v3/activity/events/types/
            string glyph = null;
            switch (value.Type)
            {
                case "CommitCommentEvent":
                    glyph = "\uf075";
                    break;
                case "CreateEvent":
                    glyph = "\uf0fe";
                    break;
                case "DeleteEvent":
                    glyph = "\uf1f8";
                    break;
                case "ForkEvent":
                    glyph = "\uf126";
                    break;
                case "GollumEvent":
                    glyph = "\uf1c9";
                    break;
                case "IssuesEvent":
                    glyph = "\uf188";
                    break;
                case "IssueCommentEvent":
                    glyph = "\uf075";
                    break;
                case "LabelEvent":
                    glyph = "\uf02c";
                    break;
                case "MemberEvent":
                    glyph = "\uf0c0";
                    break;
                case "ProjectCardEvent":
                    glyph = "\uf284";
                    break;
                case "ProjectEvent":
                    glyph = "\uf284";
                    break;
                case "PublicEvent":
                    glyph = "\uf0c0";
                    break;
                case "PullRequestEvent":
                    glyph = "\uf126";
                    break;
                case "PushEvent":
                    glyph = "\uf0ee";
                    break;
                case "ReleaseEvent":
                    glyph = "\uf1fd";
                    break;
                case "WatchEvent":
                    glyph = "\uf06e";
                    break;
                default:
                    glyph = "\uf024";
                    return string.Empty;
            }

            return glyph;
        }
    }
}
