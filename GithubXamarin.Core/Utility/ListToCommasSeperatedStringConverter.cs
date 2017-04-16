using System.Collections.Generic;
using System.Linq;
using System.Text;
using Octokit;

namespace GithubXamarin.Core.Utility
{
    public class ListToCommasSeperatedStringConverter
    {
        public static string Convert(IEnumerable<string> list)
        {
            if (list == null)
            {
                return string.Empty;
            }
            var returnString = new StringBuilder();
            foreach (var item in list)
            {
                returnString.Append(item);
                returnString.Append(", ");
            }
            returnString.Remove(returnString.Length - 2, 2);
            return returnString.ToString();
        }

        public static string Convert(IEnumerable<Label> labels)
        {
            if (!labels.Any())
            {
                return null;
            }
            var returnString = new StringBuilder();
            foreach (var label in labels)
            {
                returnString.Append(label.Name);
                returnString.Append(", ");
            }
            returnString.Remove(returnString.Length - 2, 2);
            return returnString.ToString();
        }

        public static string Convert(IReadOnlyList<User> assignees)
        {
            if (!assignees.Any())
            {
                return null;
            }
            var returnString = new StringBuilder();
            foreach (var assignee in assignees)
            {
                returnString.Append(assignee.Login);
                returnString.Append(", ");
            }
            returnString.Remove(returnString.Length - 2, 2);
            return returnString.ToString();
        }
    }
}
