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
            get => _users;
            set
            {
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        private ObservableCollection<RepositoryContributor> _contributors;
        public ObservableCollection<RepositoryContributor> Contributors
        {
            get => _contributors;
            set
            {
                _contributors = value;
                RaisePropertyChanged(() => Contributors);
            }
        }

        private ICommand _userClickCommand;
        public ICommand UserClickCommand
        {
            get
            {
                _userClickCommand = _userClickCommand ?? new MvxCommand<object>(NavigateToUserView);
                return _userClickCommand;
            }
        }

        private ICommand _contributorClickCommand;
        public ICommand ContributorClickCommand
        {
            get
            {
                _contributorClickCommand = _contributorClickCommand ??
                                           new MvxCommand<object>(NavigateToContributorView);
                return _contributorClickCommand;
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

        private bool _isContributor;
        public bool IsContributor
        {
            get => _isContributor;
            set
            {
                IsUser = !value;
                _isContributor = value;
                RaisePropertyChanged(() => IsContributor);
            }
        }

        private bool _isUser;
        public bool IsUser
        {
            get => _isUser; set
            {
                _isUser = value;
                RaisePropertyChanged(() => IsUser);
            }
        }



        private long _repositoryId;
        private UsersTypeEnumeration? _usersType;
        private string _userLogin;

        public int SelectedIndex { get; set; }
        public int SelectedContributorIndex { get; set; }

        #endregion

        public UsersViewModel(IGithubClientService githubClientService, IUserDataService userDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
        }

        public async void Init(long repositoryId, UsersTypeEnumeration usersType, string userLogin)
        {
            _repositoryId = repositoryId;
            _usersType = usersType;
            _userLogin = userLogin;
            await Refresh();
        }

        private void NavigateToUserView(object obj)
        {
            var user = obj as User ?? Users[SelectedIndex];
            if (user != null) ShowViewModel<UserViewModel>(new { userLogin = user.Login });
        }

        private void NavigateToContributorView(object obj)
        {
            var user = obj as RepositoryContributor ?? Contributors[SelectedContributorIndex];
            if (user != null) ShowViewModel<UserViewModel>(new { userLogin = user.Login });
        }

        public async Task Refresh()
        {
            if (!(await IsInternetAvailable()))
            {
                await DialogService.ShowSimpleDialogAsync("So for the time being, here is an interesting fact: If each dead person became a ghost, there’d be more than 100 billion of them haunting us all. Creepy, but cool!", "No Internet Connection!");
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
                            IsContributor = false;
                            break;

                        case UsersTypeEnumeration.Collaborators:
                            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Collaborators" });
                            Users = await _userDataService.GetCollaboratorsForRepository(_repositoryId,
                                GithubClientService.GetAuthorizedGithubClient());
                            IsContributor = false;
                            break;

                        case UsersTypeEnumeration.Contributors:
                            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Contributors" });
                            Contributors = await _userDataService.GetContributorsForRepository(_repositoryId,
                                GithubClientService.GetAuthorizedGithubClient());
                            IsContributor = true;
                            break;
                        case UsersTypeEnumeration.Followers:
                            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Followers of {_userLogin}" });
                            Users = await _userDataService.GetFollowersForUser(_userLogin,
                                GithubClientService.GetAuthorizedGithubClient());
                            IsContributor = false;
                            break;
                        case UsersTypeEnumeration.Following:
                            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Users followed by {_userLogin}" });
                            Users = await _userDataService.GetFollowingForUser(_userLogin,
                                GithubClientService.GetAuthorizedGithubClient());
                            IsContributor = false;
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
                await DialogService.ShowSimpleDialogAsync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
