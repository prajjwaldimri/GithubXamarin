using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;
// ReSharper disable MemberCanBePrivate.Global

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
                _repositoryClickCommand = _repositoryClickCommand ?? new MvxCommand<object>(NavigateToRepositoryView);
                return _repositoryClickCommand;
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
                _addCommand = _addCommand ?? new MvxCommand(GoToNewRepositoryView);
                return _addCommand;
            }
        }

        public int SelectedIndex { get; set; }

        private string _userLogin;

        #endregion


        public RepositoriesViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _repoDataService = repoDataService;
        }

        public async void Init(string userLogin)
        {
            _userLogin = userLogin;
            await Refresh();
        }
        
        private void NavigateToRepositoryView(object obj)
        {
            var repository = obj as Repository ?? Repositories[SelectedIndex];
            ShowViewModel<RepositoryViewModel>(new { repositoryId = repository.Id });
        }

        public void GoToNewRepositoryView()
        {
            ShowViewModel<NewRepositoryViewModel>();
        }

        public async Task Refresh()
        {
            if (!(await IsInternetAvailable()))
            {
                await DialogService.ShowSimpleDialogAsync("No internet, No work :(", "No Internet Connection");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            try
            {
                if (string.IsNullOrWhiteSpace(_userLogin))
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Your Repositories" });
                    Repositories =
                        await _repoDataService.GetAllRepositoriesForCurrentUser(
                            GithubClientService.GetAuthorizedGithubClient());
                }
                else
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Repositories of {_userLogin}" });
                    Repositories = await _repoDataService.GetAllRepositoriesForUser(_userLogin,
                        GithubClientService.GetAuthorizedGithubClient());
                }
            }
            catch (HttpRequestException)
            {
                await DialogService.ShowSimpleDialogAsync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        
    }
}
