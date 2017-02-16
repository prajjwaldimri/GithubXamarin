using System.Collections.ObjectModel;
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

namespace GithubXamarin.Core.ViewModels
{
    public class UsersViewModel : BaseViewModel, IUsersViewModel
    {
        #region Commands and Properties

        private readonly IUserDataService _userDataService;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public int SelectedIndex { get; set; }

        private ICommand _userClickCommand;
        public ICommand UserClickCommand
        {
            get
            {
                _userClickCommand = _userClickCommand ?? new MvxCommand(NavigateToUserView);
                return _userClickCommand;
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

        private long _repositoryId;
        private UsersTypeEnumeration? _usersType;

        #endregion

        public UsersViewModel(IGithubClientService githubClientService, IUserDataService userDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
        }

        public async void Init(long repositoryId, UsersTypeEnumeration? usersType = null)
        {
            _repositoryId = repositoryId;
            _usersType = usersType;
            await Refresh();
        }

        private void NavigateToUserView()
        {
            var user = Users?[SelectedIndex];
            if (user != null)
                ShowViewModel<UserViewModel>(new { userLogin = user.Login });
        }

        private async Task Refresh()
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("So for the time being, here is an interesting fact: If each dead person became a ghost, there’d be more than 100 billion of them haunting us all. Creepy, but cool!", "No Internet Connection!");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            try
            {
                if (_usersType != null)
                {
                    switch (_usersType)
                    {
                        case UsersTypeEnumeration.Stargazers:
                            Users = await _userDataService.GetStargazersForRepository(_repositoryId,
                                GithubClientService.GetAuthorizedGithubClient());
                            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Stargazers" });
                            break;
                        case UsersTypeEnumeration.Collaborators:
                            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Collaborators" });
                            Users = await _userDataService.GetCollaboratorsForRepository(_repositoryId,
                                GithubClientService.GetAuthorizedGithubClient());
                            break;
                        default:
                            Users = await _userDataService.GetCollaboratorsForRepository(_repositoryId,
                                GithubClientService.GetAuthorizedGithubClient());
                            break;
                    }
                }
                else
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Collaborators" });
                    Users = await _userDataService.GetCollaboratorsForRepository(_repositoryId,
                        GithubClientService.GetAuthorizedGithubClient());
                }
            }
            catch (HttpRequestException)
            {
                await DialogService.ShowDialogASync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
