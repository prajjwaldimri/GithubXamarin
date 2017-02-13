using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class UserViewModel : BaseViewModel, IUserViewModel
    {
        #region Commands and Properties

        private readonly IUserDataService _userDataService;

        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged(() => User);
            }
        }

        #endregion

        public UserViewModel(IGithubClientService githubClientService, IUserDataService userDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
        }

        public async void Init(string userLogin)
        {
            if (IsInternetAvailable())
            {
                Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
                if (string.IsNullOrWhiteSpace(userLogin))
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Your Profile" });
                    User = await _userDataService.GetCurrentUser(GithubClientService.GetAuthorizedGithubClient());
                    
                }
                else
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Profile of {userLogin}" });
                    User = await _userDataService.GetUser(userLogin, GithubClientService.GetAuthorizedGithubClient());
                }
                Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
            }
        }
    }
}
