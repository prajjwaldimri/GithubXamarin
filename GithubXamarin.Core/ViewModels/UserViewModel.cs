using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Core.ViewModels;
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

        private bool _isUserCurrent;
        public bool IsUserCurrent
        {
            get { return _isUserCurrent; }
            set
            {
                _isUserCurrent = value; 
                RaisePropertyChanged(() => IsUserCurrent);
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

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                _editCommand = _editCommand ?? new MvxCommand(GoToNewUserView);
                return _editCommand;
            }
            
        }

        private string _userLogin;

        #endregion

        public UserViewModel(IGithubClientService githubClientService, IUserDataService userDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
        }

        public async void Init(string userLogin)
        {
            if (!(string.IsNullOrWhiteSpace(userLogin)))
            {
                _userLogin = userLogin;
                IsUserCurrent = false;
            }
            else
            {
                IsUserCurrent = true;
            }
            await Refresh();
        }

        private void GoToNewUserView()
        {
            ShowViewModel<NewUserViewModel>(new
            {
                name = User.Name,
                bio = User.Bio,
                blog = User.Blog,
                email = User.Email,
                hireable = User.Hireable.ConvertToBoolean(),
                location = User.Location,
                company = User.Company
            });
        }

        private async Task Refresh()
        {
            if (!IsInternetAvailable()) return;
            try
            {
                Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
                if (string.IsNullOrWhiteSpace(_userLogin))
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Your Profile" });
                    User = await _userDataService.GetCurrentUser(GithubClientService.GetAuthorizedGithubClient());

                }
                else
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Profile of {_userLogin}" });
                    User = await _userDataService.GetUser(_userLogin, GithubClientService.GetAuthorizedGithubClient());
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
