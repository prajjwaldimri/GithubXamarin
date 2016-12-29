using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Views;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;

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
        private long _repositoryId;

        //Commands
        private DelegateCommand<ItemClickEventArgs> _issuesClickDelegateCommand;
        private DelegateCommand<ItemClickEventArgs> _forksClickDelegateCommand;
        private DelegateCommand<ItemClickEventArgs> _collaboratorsClickDelegateCommand;
        private DelegateCommand<ItemClickEventArgs> _stargazersClickDelegateCommand;

        public DelegateCommand<ItemClickEventArgs> IssuesClickDelegateCommand
            => _issuesClickDelegateCommand ?? (_issuesClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToIssues));

        public DelegateCommand<ItemClickEventArgs> ForksClickDelegateCommand
            =>
                _forksClickDelegateCommand ??
                (_forksClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToForks));

        public DelegateCommand<ItemClickEventArgs> CollaboratorsClickDelegateCommand
            =>
                _collaboratorsClickDelegateCommand ??
                (_collaboratorsClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToCollaborators));

        public DelegateCommand<ItemClickEventArgs> StargazersClickDelegateCommand
            =>
                _stargazersClickDelegateCommand ??
                (_stargazersClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToStarGazers));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Busy.SetBusy(true,"Getting Details of Repository");
            Repository = SessionState.Get<Repository>(parameter.ToString());
            RepositoryId = Repository.Id;
            _repositoryId = Repository.Id;
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
            SessionState.Remove(parameter.ToString());
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

        private async void GoToForks(ItemClickEventArgs obj)
        {
            const string key = nameof(Repository);
            SessionState.Add(key, Repository);
            await NavigationService.NavigateAsync(typeof(Views.RepositoriesPage), key);
        }

        private async void GoToStarGazers(ItemClickEventArgs obj)
        {
            var starredClient = new StarredClient(new ApiConnection(new Connection(new ProductHeaderValue("githubuwp"))));
            var starredUsers = await starredClient.GetAllStargazers(_repositoryId);
            const string key = nameof(starredUsers);
            SessionState.Add(key, starredUsers.ToObservableCollection());
            await NavigationService.NavigateAsync(typeof(Views.UsersPage), key);
        }

        private async void GoToCollaborators(ItemClickEventArgs obj)
        {
            var collaboratorsClient = new RepoCollaboratorsClient(new ApiConnection(new Connection(new ProductHeaderValue("githubuwp"))));
            var collaborators = await collaboratorsClient.GetAll(_repositoryId);
            const string key = nameof(collaborators);
            SessionState.Add(key, collaborators.ToObservableCollection());
            await NavigationService.NavigateAsync(typeof(Views.UsersPage), key);
        }
    }
}
