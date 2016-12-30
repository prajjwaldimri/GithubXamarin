using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class SearchResultsPageViewModel : ViewModelBase
    {
        public string SearchText { get; set; }

        public ComboBoxItem SearchCategory { get; set; }
        public int SearchIndex { get; set; }
        public ObservableCollection<SearchCode> CodeListItemsObservableCollection { get; set; }
        public ObservableCollection<Issue> IssuesListItemsObservableCollection { get; set; }
        public ObservableCollection<Repository> ReposListItemsObservableCollection { get; set; }
        public ObservableCollection<User> UsersListItemsObservableCollection { get; set; }
        public bool CodeListVisibility { get; set; }
        public bool IssuesListVisibility { get; set; }
        public bool ReposListVisibility { get; set; }
        public bool UsersListVisibility { get; set; }


        private GitHubClient _client;

        private DelegateCommand _querySubmittedDelegateCommand;
        private DelegateCommand _comboBoxSelectionChangedDelegateCommand;
        private DelegateCommand<ItemClickEventArgs> _issuesListItemClickDelegateCommand;
        private DelegateCommand<ItemClickEventArgs> _reposListItemClickDelegateCommand;
        private DelegateCommand<ItemClickEventArgs> _usersListItemClickDelegateCommand;


        #region PublicCommands
        public DelegateCommand QuerySubmittedDelegateCommand
            =>
                _querySubmittedDelegateCommand ??
                (_querySubmittedDelegateCommand = new DelegateCommand(GoSearch));

        public DelegateCommand ComboBoxSelectionChangedDelegateCommand
            =>
                _comboBoxSelectionChangedDelegateCommand ??
                (_comboBoxSelectionChangedDelegateCommand = new DelegateCommand(SelectionChangedHandler));

        public DelegateCommand<ItemClickEventArgs> IssuesListItemClickDelegateCommand
            =>
               _issuesListItemClickDelegateCommand ??
                (_issuesListItemClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToIssue));

        public DelegateCommand<ItemClickEventArgs> ReposListItemClickDelegateCommand
            =>
               _reposListItemClickDelegateCommand ??
                (_reposListItemClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToRepo));

        public DelegateCommand<ItemClickEventArgs> UsersListItemClickDelegateCommand
            =>
               _usersListItemClickDelegateCommand ??
                (_usersListItemClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToUser));

        #endregion


        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            SearchIndex = 0;
            if (SessionState.Get<GitHubClient>("GitHubClient") != null)
            {
                _client = SessionState.Get<GitHubClient>("GitHubClient");
            }
            else
            {
                _client = new GitHubClient(new ProductHeaderValue("githubuwp"));
                SessionState.Add("GitHubClient", _client);
            }
            return Task.CompletedTask;
        }

        private Task VisibilityUpdater()
        {
            CodeListVisibility = false;
            IssuesListVisibility = false;
            ReposListVisibility = false;
            UsersListVisibility = false;
            switch (SearchCategory.Content.ToString())
            {
                case "Code":
                    CodeListVisibility = true;
                    break;
                case "Issues":
                    IssuesListVisibility = true;
                    break;
                case "Repos":
                    ReposListVisibility = true;
                    break;
                case "Users":
                    UsersListVisibility = true;
                    break;
                default:
                    return Task.CompletedTask;
            }
            RaisePropertyChanged(string.Empty);
            return Task.CompletedTask;
        }

        private async void SelectionChangedHandler()
        {
            await VisibilityUpdater();
        }

        private async void GoSearch()
        {
            Views.Busy.SetBusy(true,"Searching...");
            if (string.IsNullOrWhiteSpace(SearchText)) return;
            var searchClient = new SearchClient(new ApiConnection(_client.Connection));
            await VisibilityUpdater();
            switch (SearchCategory.Content.ToString())
            {
                case "Code":
                    CodeListItemsObservableCollection = (await searchClient.SearchCode(new SearchCodeRequest(SearchText))).Items.ToObservableCollection();
                    break;
                case "Issues":
                    IssuesListItemsObservableCollection = (await searchClient.SearchIssues(new SearchIssuesRequest(SearchText))).Items.ToObservableCollection();
                    break;
                case "Repos":
                    ReposListItemsObservableCollection = (await searchClient.SearchRepo(new SearchRepositoriesRequest(SearchText))).Items.ToObservableCollection();
                    break;
                case "Users":
                    UsersListItemsObservableCollection = (await searchClient.SearchUsers(new SearchUsersRequest(SearchText))).Items.ToObservableCollection();
                    break;
                default:
                    return;
            }
            RaisePropertyChanged(string.Empty);
            Views.Busy.SetBusy(false);
        }

        private async void GoToIssue(ItemClickEventArgs obj)
        {
            var issue = (Issue)obj.ClickedItem;
            const string key = nameof(issue);
            SessionState.Add(key, issue);
            await NavigationService.NavigateAsync(typeof(Views.IssuePage), key);
        }

        private async void GoToRepo(ItemClickEventArgs obj)
        {
            var clickedRepository = (Repository)obj.ClickedItem;
            const string key = nameof(clickedRepository);
            SessionState.Add(key, clickedRepository);
            await NavigationService.NavigateAsync(typeof(Views.RepositoryPage), key);
        }

        private async void GoToUser(ItemClickEventArgs obj)
        {
            var user = (User)obj.ClickedItem;
            const string key = nameof(user);
            SessionState.Add(key, user);
            await NavigationService.NavigateAsync(typeof(Views.UserProfilePage), key);
        }
    }
}
