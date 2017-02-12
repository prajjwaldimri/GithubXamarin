using System;
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

        private int _filterSelectedIndex;
        public int FilterSelectedIndex
        {
            get { return _filterSelectedIndex;}
            set
            {
                _filterSelectedIndex = value;
                RaisePropertyChanged(() => FilterSelectedIndex);
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
            IssueClickCommand = new MvxCommand(GoToIssue);
            RepositoryClickCommand = new MvxCommand(GoToRepository);
            UserClickCommand = new MvxCommand(GoToUser);
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

        private void GoToIssue()
        {
            var issue = Issues[IssuesSelectedIndex];
            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = issue.Number,
                owner = issue.User.Login,
                repoName = HtmlUrlToRepositoryNameConverter.Convert
                (issue.HtmlUrl.AbsoluteUri, issue.User.Login)
            });
        }

        private void GoToRepository()
        {
            ShowViewModel<RepositoryViewModel>(new { repositoryId = Repositories[RepositoriesSelectedIndex].Id });
        }

        private void GoToUser()
        {
            ShowViewModel<UserViewModel>(new { userLogin = Users[UsersSelectedIndex].Login });
        }

        private async Task Search(string searchTerm, SearchTypeEnumeration searchType)
        {
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
