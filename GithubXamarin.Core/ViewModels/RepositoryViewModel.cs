using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class RepositoryViewModel : BaseViewModel, IRepositoryViewModel
    {
        #region Commands and Properties

        private readonly IRepoDataService _repoDataService;

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

        #endregion

        public RepositoryViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _repoDataService = repoDataService;
        }

        public async void Init(long repositoryId)
        {
            if (IsInternetAvailable())
            {
                Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
                Repository = await _repoDataService.GetRepository(repositoryId,
                    GithubClientService.GetAuthorizedGithubClient());
                await CheckRepositoryStats();
                Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
            }
        }

        /// <summary>
        /// Checks for repository stats which are not directly available in
        /// </summary>
        /// <returns></returns>
        private async Task CheckRepositoryStats()
        {
            //Check if repository is starred
            var starredClient = new StarredClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            IsRepositoryStarred = await starredClient.CheckStarred(Repository.Owner.ToString(), Repository.Name);
        }

        /// <summary>
        /// Create a fork of the current repository
        /// </summary>
        private void ForkRepository()
        {
            var forkClient = new RepositoryForksClient(new ApiConnection(GithubClientService.GetAuthorizedGithubClient().Connection));
            forkClient.Create(Repository.Id, new NewRepositoryFork());
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
            
        }
    }
}
