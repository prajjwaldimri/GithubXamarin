using System.Collections.ObjectModel;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class RepositoriesViewModel : BaseViewModel, IRepositoriesViewModel
    {
        #region Commands and Properties

        private readonly IRepoDataService _repoDataService;

        private ObservableCollection<Repository> _repositories;
        public ObservableCollection<Repository> Repositories
        {
            get { return _repositories; }
            set
            {
                _repositories = value;
                RaisePropertyChanged(() => Repositories);
            }
        }

        private ICommand _repositoryClickCommand;
        public ICommand RepositoryClickCommand
        {
            get
            {
                _repositoryClickCommand = _repositoryClickCommand ?? new MvxCommand(NavigateToRepositoryView);
                return _repositoryClickCommand;
            }
        }

        public int SelectedIndex { get; set; }

        #endregion


        public RepositoriesViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _repoDataService = repoDataService;
        }

        /// <summary>
        /// Navigates To the repository ViewModel
        /// </summary>
        private void NavigateToRepositoryView()
        {
            var repository = Repositories?[SelectedIndex];
            if (repository != null)
            {
                ShowViewModel<RepositoryViewModel>(new { repositoryId = repository.Id });
            }
        }

        public async void Init(string userLogin)
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("What is better ? To be born good or to overcome your evil nature through great effort ?", "No Internet Connection!");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            if (string.IsNullOrWhiteSpace(userLogin))
            {
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Your Repositories" });
                Repositories =
                    await _repoDataService.GetAllRepositoriesForCurrentUser(
                        GithubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Repositories of {userLogin}" });
                Repositories = await _repoDataService.GetAllRepositoriesForUser(userLogin,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
