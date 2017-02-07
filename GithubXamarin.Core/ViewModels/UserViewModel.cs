using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Plugins.Network.Reachability;
using MvvmCross.Plugins.Network.Rest;
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

        public UserViewModel(IGithubClientService githubClientService, IUserDataService userDataService) : base(githubClientService)
        {
            _userDataService = userDataService;
        }

        public async void Init(string userLogin)
        {
            if (IsInternetAvailable())
            {
                if (string.IsNullOrWhiteSpace(userLogin))
                {
                    User = await _userDataService.GetCurrentUser(GithubClientService.GetAuthorizedGithubClient());

                }
                else
                {
                    User = await _userDataService.GetUser(userLogin, GithubClientService.GetAuthorizedGithubClient());
                }
            }
        }
    }
}
