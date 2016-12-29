using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Views;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class RepositoryPageViewModel : ViewModelBase
    {
        public double RepositoryId { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryReadme { get; set; }
        public string RepositoryOwner { get; set; }
        public string RepositoryWebsite { get; set; }
        public string RepositoryDescription { get; set; }
        public int StarCount { get; set; }
        public int ForkCount { get; set; }

        private Repository Repository { get; set; }

        //Commands
        private DelegateCommand<ItemClickEventArgs> _issuesClickDelegateCommand;

        public DelegateCommand<ItemClickEventArgs> IssuesClickDelegateCommand
            => _issuesClickDelegateCommand ?? (_issuesClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToIssues));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Busy.SetBusy(true,"Getting Details of Repository");
            Repository = SessionState.Get<Repository>(parameter.ToString());
            SessionState.Remove(parameter.ToString());
            RepositoryId = Repository.Id;
            RepositoryName = Repository.FullName;
            GitHubClient client;
            if (SessionState.Get<GitHubClient>("GitHubClient") != null)
            {
                client = SessionState.Get<GitHubClient>("GitHubClient");
            }
            else
            {
                client = new GitHubClient(new ProductHeaderValue("githubuwp"));
                SessionState.Add("GitHubClient", client);
            }
            RepositoryOwner = Repository.Owner.Login;
            RepositoryWebsite = Repository.Homepage;
            RepositoryDescription = Repository.Description;
            StarCount = Repository.StargazersCount;
            ForkCount = Repository.ForksCount;
            RaisePropertyChanged(String.Empty);
            Busy.SetBusy(false);
        }

        /// <summary>
        /// Goes To the issue page
        /// </summary>
        /// <param name="obj"></param>
        private async void GoToIssues(ItemClickEventArgs obj)
        {
            const string key = nameof(Repository);
            SessionState.Add(key, Repository);
            await NavigationService.NavigateAsync(typeof(Views.IssuesPage), key);
        }
    }
}
