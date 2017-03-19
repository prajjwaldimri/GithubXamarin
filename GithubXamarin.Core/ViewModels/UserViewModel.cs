using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class UserViewModel : BaseViewModel, IUserViewModel
    {
        #region Commands and Properties

        private readonly IUserDataService _userDataService;
        private readonly IShareService _shareService;

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

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? new MvxAsyncCommand(async () => await Refresh());
                return _refreshCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                _shareCommand = _shareCommand ?? new MvxAsyncCommand(ShareUser);
                return _shareCommand;
            }
        }

        private string _userLogin;

        #endregion

        public UserViewModel(IGithubClientService githubClientService, IUserDataService userDataService, IMvxMessenger messenger, IDialogService dialogService, IShareService shareService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
            _shareService = shareService;
        }

        public async void Init(string userLogin)
        {
            _userLogin = userLogin;
            await Refresh();
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
                await DialogService.ShowSimpleDialogAsync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async Task ShareUser()
        {
            if (User == null) return;
            await _shareService.ShareLinkAsync(new Uri(User.HtmlUrl), User.Name);
        }
    }
}
