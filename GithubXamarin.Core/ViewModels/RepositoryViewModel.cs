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
            get { return _repository;}
            set
            {
                _repository = value;
                RaisePropertyChanged(() => Repository);
            }
        }

        private bool _isRepositoryStarred;
        public bool IsRepositoryStarred
        {
            get { return _isRepositoryStarred;}
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

        public RepositoryViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService, IMvxMessenger messenger, IDialogService dialogService, IShareService shareService) : base(githubClientService, messenger, dialogService)
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
        /// Checks for repository stats which are not directly available in
        /// </summary>
        /// <returns></returns>
        private async Task CheckRepositoryStats()
        {
            //Check if repository is starred
            var starredClient = new StarredClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            IsRepositoryStarred = await starredClient.CheckStarred(Repository.Owner.Login.ToString(), Repository.Name);
        }

        /// <summary>
        /// Create a fork of the current repository
        /// </summary>
        private void ForkRepository()
        {
            var forkClient = new RepositoryForksClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            forkClient.Create(Repository.Id, new NewRepositoryFork());
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
            ShowViewModel<UsersViewModel>(new {repositoryId = Repository.Id, usersType = UsersTypeEnumeration.Collaborators});
        }

        /// <summary>
        /// Navigates To UsersView and shows all the stargazers of current Repository
        /// </summary>
        private void ShowStargazersOfRepository()
        {
            ShowViewModel<UsersViewModel>(new { repositoryId = Repository.Id, usersType = UsersTypeEnumeration.Stargazers });
        }

        /// <summary>
        /// Shows issues of current Repository
        /// </summary>
        private void ShowIssuesOfRepository()
        {
            ShowViewModel<IssuesViewModel>(new {repositoryId = Repository.Id});
        }

        /// <summary>
        /// Shows the Readme of current Repository
        /// </summary>
        private void NavigateToReadme()
        {
            ShowViewModel<FileViewModel>(new {fileType = FileTypeEnumeration.Readme, repositoryId = Repository.Id});
        }

        private async Task GoToNewIssueView()
        {
            if (!IsInternetAvailable()) return;
            ShowViewModel<NewIssueViewModel>(new
            {
                repositoryId = Repository.Id
            });

        }

        private async Task Refresh()
        {
            if (!IsInternetAvailable()) return;
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            try
            {
                Repository = await _repoDataService.GetRepository(_repositoryId,
                    GithubClientService.GetAuthorizedGithubClient());
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"{Repository.FullName}" });
            }
            catch (HttpRequestException)
            {
                await DialogService.ShowDialogASync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
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
