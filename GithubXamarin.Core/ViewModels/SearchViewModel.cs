using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using GithubXamarin.Core.Utility;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class SearchViewModel : BaseViewModel, ISearchResultViewModel
    {
        #region Properties and Commands

        private readonly IRepoDataService _repoDataService;
        private readonly IUserDataService _userDataService;
        private readonly IIssueDataService _issueDataService;

        public ICommand SearchCommand { get; set; }
        public ICommand IssueClickCommand { get; set; }
        public ICommand RepositoryClickCommand { get; set; }
        public ICommand UserClickCommand { get; set; }
        public ICommand FilterIndexUpdaterCommand { get; set; }

        private int _filterSelectedIndex;
        public int FilterSelectedIndex
        {
            get { return _filterSelectedIndex;}
            set
            {
                _filterSelectedIndex = value;
                switch (value)
                {
                    case 0:
                        IssuesListVisibility = true;
                        RepositoriesListVisibility = false;
                        UsersListVisibility = false;
                        break;
                    case 1:
                        IssuesListVisibility = false;
                        RepositoriesListVisibility = true;
                        UsersListVisibility = false;
                        break;
                    case 2:
                        IssuesListVisibility = false;
                        RepositoriesListVisibility = false;
                        UsersListVisibility = true;
                        break;
                }
                RaisePropertyChanged(() => FilterSelectedIndex);
                RaisePropertyChanged(() => IssuesListVisibility);
                RaisePropertyChanged(() => RepositoriesListVisibility);
                RaisePropertyChanged(() => UsersListVisibility);
            }
        }

        private string _searchBoxText;
        public string SearchBoxText
        {
            get { return _searchBoxText;}
            set { _searchBoxText = value; RaisePropertyChanged(() => SearchBoxText); }
        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users;}
            set { _users = value; RaisePropertyChanged(() => Users); }
        }

        private ObservableCollection<Issue> _issues;
        public ObservableCollection<Issue> Issues
        {
            get { return _issues; }
            set { _issues = value; RaisePropertyChanged(() => Issues); }
        }

        private ObservableCollection<Repository> _repositories;
        public ObservableCollection<Repository> Repositories
        {
            get { return _repositories; }
            set { _repositories = value; RaisePropertyChanged(() => Repositories); }
        }

        private int _issuesSelectedIndex;
        public int IssuesSelectedIndex
        {
            get { return _issuesSelectedIndex; }
            set { _issuesSelectedIndex = value; RaisePropertyChanged(() => IssuesSelectedIndex); }
        }

        private int _repositoriesSelectedIndex;
        public int RepositoriesSelectedIndex
        {
            get { return _repositoriesSelectedIndex;}
            set { _repositoriesSelectedIndex = value; RaisePropertyChanged(() => RepositoriesSelectedIndex); }
        }

        private int _usersSelectedIndex;
        public int UsersSelectedIndex
        {
            get { return _usersSelectedIndex;}
            set { _usersSelectedIndex = value; RaisePropertyChanged(() => UsersSelectedIndex); }
        }

        public bool IssuesListVisibility { get; set; } = true;
        public bool RepositoriesListVisibility { get; set; } = false;
        public bool UsersListVisibility { get; set; } = false;

        public List<string> SearchCategories { get; } = new List<string>()
        {
            "Issues", "Repositories", "Users"
        };

        #endregion

        public SearchViewModel(IRepoDataService repoDataService, IUserDataService userDataService,
            IIssueDataService issueDataService, IGithubClientService githubClientService, 
            IMvxMessenger messenger, IDialogService dialogService) 
            : base(githubClientService, messenger, dialogService)
        {
            _repoDataService = repoDataService;
            _userDataService = userDataService;
            _issueDataService = issueDataService;

            SearchCommand = new MvxCommand(ExecuteSearch);

            IssueClickCommand = new MvxCommand<object>(GoToIssue);
            RepositoryClickCommand = new MvxCommand<object>(GoToRepository);
            UserClickCommand = new MvxCommand<object>(GoToUser);

            FilterIndexUpdaterCommand = new MvxCommand<string>(UpdateFilterIndex);
        }

        public async void Init(string searchTerm, SearchTypeEnumeration searchType)
        {
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            SearchBoxText = searchTerm;
            await Search(searchTerm, searchType);
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async void ExecuteSearch()
        {
            switch (FilterSelectedIndex)
            {
                case 0:
                    await Search(SearchBoxText, SearchTypeEnumeration.Issues);
                    break;
                case 1:
                    await Search(SearchBoxText, SearchTypeEnumeration.Repositories);
                    break;
                case 2:
                    await Search(SearchBoxText, SearchTypeEnumeration.Users);
                    break;
                default:
                    break;
            }
        }

        private void GoToIssue(object obj)
        {
            if (IssuesSelectedIndex < 0) { return; }
            var issue = obj as Issue ?? Issues[IssuesSelectedIndex];
            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = issue.Number,
                owner = issue.HtmlUrl.Segments[1].Remove(issue.HtmlUrl.Segments[1].Length-1),
                repoName = issue.HtmlUrl.Segments[2].Remove(issue.HtmlUrl.Segments[2].Length-1)
            });
        }

        private void GoToRepository(object obj)
        {
            if (RepositoriesSelectedIndex < 0) { return; }
            var repository = obj as Repository ?? Repositories[RepositoriesSelectedIndex];
            ShowViewModel<RepositoryViewModel>(new { repositoryId = repository.Id });
        }

        private void GoToUser(object obj)
        {
            if (UsersSelectedIndex < 0) { return; }
            var user = obj as User ?? Users[UsersSelectedIndex];
            ShowViewModel<UserViewModel>(new { userLogin = user.Login });
        }

        private void UpdateFilterIndex(string obj)
        {
            switch (obj)
            {
                case "Issues":
                    FilterSelectedIndex = 0;
                    break;
                case "Users":
                    FilterSelectedIndex = 1;
                    break;
                case "Repositories":
                    FilterSelectedIndex = 2;
                    break;
            }
        }

        private async Task Search(string searchTerm, SearchTypeEnumeration searchType)
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("Or is it?", "Internet is not available");
                return;
            }
            switch (searchType)
            {
                case SearchTypeEnumeration.Issues:
                    Issues = await _issueDataService.SearchIssues(searchTerm,
                        GithubClientService.GetAuthorizedGithubClient());
                    break;
                case SearchTypeEnumeration.Repositories:
                    Repositories = await _repoDataService.SearchRepositories(searchTerm,
                        GithubClientService.GetAuthorizedGithubClient());
                    break;
                case SearchTypeEnumeration.Users:
                    Users = await _userDataService.SearchUsers(searchTerm,
                        GithubClientService.GetAuthorizedGithubClient());
                    break;
                case SearchTypeEnumeration.Code:
                    break;
                default:
                    break;
            }
        }
    }
}
