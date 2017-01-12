using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class IssuePageViewModel : ViewModelBase
    {
        public int IssueNumber { get; set; }
        public string IssueName { get; set; }
        public string IssueState { get; set; }
        public string IssueTitle { get; set; }
        public string IssueBody { get; set; }
        public DateTime IssueCreatedAt { get; set; }
        public User IssueCreator { get; set; }
        public User AssignedUser { get; set; }
        public DateTime ClosedAt { get; set; }
        public User ClosedBy { get; set; }
        public int CommentsCount { get; set; }
        public IssueComment IssueComments { get; set; }
        public ObservableCollection<Label> Labels { get; set; }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            var issue = SessionState.Get<Issue>(parameter.ToString());
            IssueNumber = issue.Number;
            IssueName = issue.Title;
            IssueState = issue.State.ToString();
            IssueTitle = issue.Title;
            IssueBody = issue.Body;
            AssignedUser = issue.User;
            if (issue.State == ItemState.Closed)
            {
                // ReSharper disable once PossibleInvalidOperationException
                // Checked above ^ already
                ClosedAt = issue.ClosedAt.Value.DateTime;
                ClosedBy = issue.ClosedBy;
            }
            Labels = issue.Labels.ToObservableCollection();
            IssueCreatedAt = issue.CreatedAt.DateTime;
            IssueCreator = issue.User;
            CommentsCount = issue.Comments;
            RaisePropertyChanged(string.Empty);
            return Task.CompletedTask;
        }
    }
}
