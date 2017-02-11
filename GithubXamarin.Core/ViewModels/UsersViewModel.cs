using System.Collections.ObjectModel;
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

        #endregion

        public UsersViewModel(IGithubClientService githubClientService, IUserDataService userDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
        }

        public async void Init(long repositoryId, UsersTypeEnumeration? usersType = null)
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("So for the time being, here is an interesting fact: If each dead person became a ghost, there’d be more than 100 billion of them haunting us all. Creepy, but cool!", "No Internet Connection!");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            if (usersType != null)
            {
                switch (usersType)
                {
                    case UsersTypeEnumeration.Stargazers:
                        Users = await _userDataService.GetStargazersForRepository(repositoryId,
                            GithubClientService.GetAuthorizedGithubClient());
                        Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Stargazers"});
                        break;
                    case UsersTypeEnumeration.Collaborators:
                        Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Collaborators" });
                        Users = await _userDataService.GetCollaboratorsForRepository(repositoryId,
                            GithubClientService.GetAuthorizedGithubClient());
                        break;
                    default:
                        Users = await _userDataService.GetCollaboratorsForRepository(repositoryId,
                            GithubClientService.GetAuthorizedGithubClient());
                        break;
                }
            }
            else
            {
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Collaborators" });
                Users = await _userDataService.GetCollaboratorsForRepository(repositoryId,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        public void NavigateToUserView()
        {
            var user = Users?[SelectedIndex];
            if (user != null)
                ShowViewModel<UserViewModel>(new { userLogin = user.Login });
        }
    }
}
