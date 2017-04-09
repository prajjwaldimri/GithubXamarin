using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

// ReSharper disable MemberCanBePrivate.Global

namespace GithubXamarin.Core.ViewModels
{
    public class RepositoryViewModel : BaseViewModel, IRepositoryViewModel
    {
        #region Commands and Properties

        private readonly IRepoDataService _repoDataService;
        private readonly IShareService _shareService;

        private Repository _repository;
        public Repository Repository
        {
            get => _repository;
            set
            {
                _repository = value;
                RaisePropertyChanged(() => Repository);
            }
        }

        private bool _isRepositoryStarred;
        public bool IsRepositoryStarred
        {
            get => _isRepositoryStarred;
            set
            {
                _isRepositoryStarred = value;
                RaisePropertyChanged(() => IsRepositoryStarred);
            }
        }

        private ICommand _forkClickCommand;
        public ICommand ForkClickCommand
        {
            get
            {
                _forkClickCommand = _forkClickCommand ?? new MvxCommand(ForkRepository);
                return _forkClickCommand;
            }
        }

        private ICommand _starClickCommand;
        public ICommand StarClickCommand
        {
            get
            {
                _starClickCommand = _starClickCommand ?? new MvxAsyncCommand(async () => await StarOrUnstarRepository());
                return _starClickCommand;
            }
        }

        private ICommand _readmeClickCommand;
        public ICommand ReadmeClickCommand
        {
            get
            {
                _readmeClickCommand = _readmeClickCommand ?? new MvxCommand(NavigateToReadme);
                return _readmeClickCommand;
            }
        }

        private ICommand _collaboratorsClickCommand;
        public ICommand CollaboratorsClickCommand
        {
            get
            {
                _collaboratorsClickCommand = _collaboratorsClickCommand ?? new MvxCommand(ShowCollaboratorsOfRepository);
                return _collaboratorsClickCommand;
            }
        }

        private ICommand _stargazersClickCommand;
        public ICommand StargazersClickCommand
        {
            get
            {
                _stargazersClickCommand = _stargazersClickCommand ?? new MvxCommand(ShowStargazersOfRepository);
                return _stargazersClickCommand;
            }
        }

        private ICommand _issuesClickCommand;
        public ICommand IssuesClickCommand
        {
            get
            {
                _issuesClickCommand = _issuesClickCommand ?? new MvxCommand(ShowIssuesOfRepository);
                return _issuesClickCommand;
            }
        }

        private ICommand _contentClickCommand;
        public ICommand ContentClickCommand
        {
            get
            {
                _contentClickCommand = _contentClickCommand ?? new MvxAsyncCommand(async () => await GoToContentView());
                return _contentClickCommand;
            }
        }


        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? new MvxAsyncCommand(async () => await Refresh());
                return _refreshCommand;
            }
        }

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                _addCommand = _addCommand ?? new MvxAsyncCommand(GoToNewIssueView);
                return _addCommand;
            }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                _editCommand = _editCommand ?? new MvxAsyncCommand(GoToNewRepositoryView);
                return _editCommand;
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                _deleteCommand = _deleteCommand ?? new MvxAsyncCommand(DeleteRepository);
                return _deleteCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                _shareCommand = _shareCommand ?? new MvxAsyncCommand(ShareRepository);
                return _shareCommand;
            }
        }

        private long _repositoryId;

        #endregion

        public RepositoryViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService,
            IMvxMessenger messenger, IDialogService dialogService, IShareService shareService)
            : base(githubClientService, messenger, dialogService)
        {
            _repoDataService = repoDataService;
            _shareService = shareService;
        }

        public async void Init(long repositoryId)
        {
            _repositoryId = repositoryId;
            await Refresh();
        }

        /// <summary>
        /// Checks for repository stats which are not directly available when using Refresh Function
        /// </summary>
        /// <returns></returns>
        private async Task CheckRepositoryStats()
        {
            //Check if repository is starred
            var starredClient = new StarredClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            IsRepositoryStarred = await starredClient.CheckStarred(Repository.Owner.Login.ToString(), Repository.Name);
        }

        private void ForkRepository()
        {
            var forkClient = new RepositoryForksClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            forkClient.Create(Repository.Id, new NewRepositoryFork());
        }

        private async Task DeleteRepository()
        {
            if (!(await IsInternetAvailable()) || Repository == null) return;

            if (
                await DialogService.ShowBooleanDialogAsync(
                    $"This action CANNOT be undone. This will permanently delete the {Repository.FullName} repository, " +
                    $"wiki, issues, and comments, and remove all collaborator associations.",
                    "Are you ABSOLUTELY sure?"))
            {
                Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
                var result = await _repoDataService.DeleteRepository(Repository.Id,
                    GithubClientService.GetAuthorizedGithubClient());
                if (result)
                {
                    await DialogService.ShowSimpleDialogAsync("I hope you knew what you were doing.", "Repository deleted!");
                    Close(this);
                }
                else
                {
                    await DialogService.ShowSimpleDialogAsync("Well you got a second chance to think this through",
                        "Error in deleting Repository!");
                }
                Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
            }
        }

        private async Task StarOrUnstarRepository()
        {
            if (IsRepositoryStarred)
            {
                await _repoDataService.UnStarRepository(Repository.Owner.Login, Repository.Name,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                await _repoDataService.StarRepository(Repository.Owner.Login, Repository.Name,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            await Refresh();
        }

        /// <summary>
        /// Navigates To UsersView and shows all the collaborators of current Repository
        /// </summary>
        private void ShowCollaboratorsOfRepository()
        {
            if (Repository != null)
                ShowViewModel<UsersViewModel>(new
                {
                    repositoryId = Repository.Id,
                    usersType = UsersTypeEnumeration.Collaborators
                });
        }

        /// <summary>
        /// Navigates To UsersView and shows all the stargazers of current Repository
        /// </summary>
        private void ShowStargazersOfRepository()
        {
            if (Repository != null)
                ShowViewModel<UsersViewModel>(new
                {
                    repositoryId = Repository.Id,
                    usersType = UsersTypeEnumeration.Stargazers
                });
        }

        /// <summary>
        /// Shows issues of current Repository
        /// </summary>
        private void ShowIssuesOfRepository()
        {
            if (Repository != null) ShowViewModel<IssuesViewModel>(new { repositoryId = Repository.Id });
        }

        /// <summary>
        /// Shows the Readme of current Repository
        /// </summary>
        private void NavigateToReadme()
        {
            if (Repository != null)
                ShowViewModel<FileViewModel>(new { fileType = FileTypeEnumeration.Readme, repositoryId = Repository.Id });
        }

        private async Task GoToNewIssueView()
        {
            if (!(await IsInternetAvailable())) return;
            if (Repository != null)
                ShowViewModel<NewIssueViewModel>(new
                {
                    repositoryId = Repository.Id
                });
        }

        public async Task GoToNewRepositoryView()
        {
            if (!(await IsInternetAvailable())) return;
            if (Repository != null)
                ShowViewModel<NewRepositoryViewModel>(new
                {
                    repositoryId = Repository.Id,
                    name = Repository.Name,
                    description = Repository.Description,
                    homePage = Repository.Homepage,
                    isPrivate = Repository.Private,
                    hasIssues = Repository.HasIssues,
                    hasWiki = Repository.HasWiki
                });
        }

        public async Task GoToContentView()
        {
            if (!(await IsInternetAvailable())) return;
            if (Repository != null)
                ShowViewModel<RepositoryContentsViewModel>(new
                {
                    repositoryId = Repository.Id
                });
        }

        public async Task Refresh()
        {
            if (!(await IsInternetAvailable())) return;
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            try
            {
                Repository = await _repoDataService.GetRepository(_repositoryId,
                    GithubClientService.GetAuthorizedGithubClient());
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"{Repository.FullName}" });
            }
            catch (HttpRequestException)
            {
                await DialogService.ShowSimpleDialogAsync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            await CheckRepositoryStats();
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async Task ShareRepository()
        {
            if (Repository == null) return;
            _shareService.ShareLinkAsync(new Uri(Repository.HtmlUrl), Repository.FullName);
        }
    }
}
