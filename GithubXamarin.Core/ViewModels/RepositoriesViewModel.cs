using System.Collections.ObjectModel;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
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


        public RepositoriesViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService, IMvxMessenger messenger) : base(githubClientService, messenger)
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
                ShowViewModel<RepositoryViewModel>(new {repositoryId= repository.Id});
            }
        }

        public async void Init(string userLogin)
        {
            if (IsInternetAvailable())
            {
                if (string.IsNullOrWhiteSpace(userLogin))
                {
                    Repositories =
                        await _repoDataService.GetAllRepositoriesForCurrentUser(
                            GithubClientService.GetAuthorizedGithubClient());
                }
                else
                {
                    Repositories = await _repoDataService.GetAllRepositoriesForUser(userLogin,
                        GithubClientService.GetAuthorizedGithubClient());
                }
            }
        }
    }
}
