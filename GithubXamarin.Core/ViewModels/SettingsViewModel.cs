using System.Collections.ObjectModel;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;
using Plugin.SecureStorage;

namespace GithubXamarin.Core.ViewModels
{
    public class SettingsViewModel : BaseViewModel, ISettingsViewModel
    {
        #region Properties and Commands

        private readonly IUserDataService _userDataService;
        private readonly IFileDataService _fileDataService;

        private const long GithubXamarinRepositoryId = 73414278;

        private ICommand _loginOutButtonClickCommand;
        public ICommand LoginOutButtonClickCommand
        {
            get
            {
                _loginOutButtonClickCommand = _loginOutButtonClickCommand ?? new MvxCommand(LoginOrLogout);
                return _loginOutButtonClickCommand;
            }
        }

        private ObservableCollection<RepositoryContributor> _contributors;
        public ObservableCollection<RepositoryContributor> Contributors
        {
            get => _contributors;
            set { _contributors = value; RaisePropertyChanged(() => Contributors); }
        }

        private string _license;
        public string License
        {
            get => _license;
            set { _license = value; RaisePropertyChanged(() => License); }
        }

        private string _loginButtonContent;
        public string LoginButtonContent
        {
            get => _loginButtonContent;
            set { _loginButtonContent = value; RaisePropertyChanged(() => LoginButtonContent); }
        }

        private string _coreLimit;
        public string CoreLimit
        {
            get => _coreLimit;
            set { _coreLimit = value; RaisePropertyChanged(() => CoreLimit); }
        }

        private string _coreRemaining;
        public string CoreRemaining
        {
            get => _coreRemaining;
            set { _coreRemaining = value; RaisePropertyChanged(() => CoreRemaining); }
        }

        private string _coreReset;
        public string CoreReset
        {
            get => _coreReset;
            set
            {
                _coreReset = value;
                RaisePropertyChanged(() => CoreReset);
            }
        }

        private string _searchLimit;
        public string SearchLimit
        {
            get => _searchLimit;
            set { _searchLimit = value; RaisePropertyChanged(() => SearchLimit); }
        }

        private string _searchRemaining;
        public string SearchRemaining
        {
            get => _searchRemaining;
            set { _searchRemaining = value; RaisePropertyChanged(() => SearchRemaining); }
        }

        private string _searchReset;
        public string SearchReset
        {
            get => _searchReset;
            set
            {
                _searchReset = value;
                RaisePropertyChanged(() => SearchReset);
            }
        }
        #endregion


        public SettingsViewModel(IUserDataService userDataService, IFileDataService fileDataService,
            IGithubClientService githubClientService, IMvxMessenger messenger,
            IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
            _fileDataService = fileDataService;
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Settings" });
        }

        public async void Init()
        {
            if (!(await IsInternetAvailable()))
                return;

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            Contributors = await _userDataService.GetContributorsForRepository(GithubXamarinRepositoryId,
                GithubClientService.GetAuthorizedGithubClient());

            License = (await _fileDataService.GetFile(GithubXamarinRepositoryId, "LICENSE",
                GithubClientService.GetAuthorizedGithubClient())).Content;

            //https://developer.github.com/v3/rate_limit/
            var rateLimits = await GithubClientService.GetAuthorizedGithubClient().Miscellaneous.GetRateLimits();
            CoreLimit = $"Limit:  {rateLimits.Resources.Core.Limit}";
            CoreRemaining = $"Remaining: {rateLimits.Resources.Core.Remaining}";
            CoreReset = $"Reset:  {rateLimits.Resources.Core.Reset}";
            SearchLimit = $"Limit:  {rateLimits.Resources.Search.Limit}";
            SearchRemaining = $"Remaining:  {rateLimits.Resources.Search.Remaining}";
            SearchReset = $"Reset:  {rateLimits.Resources.Search.Reset}";

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        public override void Start()
        {
            base.Start();
            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                LoginButtonContent = "LogOut";
            }
        }

        private void LoginOrLogout()
        {
            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                CrossSecureStorage.Current.DeleteKey("OAuthToken");
                LoginButtonContent = "Login";
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }
        }
    }
}
