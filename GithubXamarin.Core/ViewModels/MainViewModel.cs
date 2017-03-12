using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Messenger;
using Octokit;
using Plugin.SecureStorage;

namespace GithubXamarin.Core.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        #region Properties and Commands

        public IEnumerable<string> MenuItems { get; private set; } = new[] { "Option1", "Option2" };

        private string _pageHeader;
        public string PageHeader
        {
            get { return _pageHeader; }
            set
            {
                _pageHeader = value;
                RaisePropertyChanged(() => PageHeader);
            }
        }

        public ICommand HamburgerMenuNavigationCommand { get; set; }
        public ICommand GoToSettingsCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand GoToUserProfileCommand { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        private string _searchBoxText;
        public string SearchBoxText
        {
            get { return _searchBoxText; }
            set { _searchBoxText = value; RaisePropertyChanged(() => SearchBoxText); }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set { _user = value; RaisePropertyChanged(() => User); }
        }

        private readonly MvxSubscriptionToken _loadingStatusMessageToken;
        private readonly MvxSubscriptionToken _appBarHeaderChangeMessageToken;

        #endregion

        public MainViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            HamburgerMenuNavigationCommand = new MvxCommand<int>(NavigateToViewModel);
            GoToSettingsCommand = new MvxCommand(ShowSettings);
            SearchCommand = new MvxCommand(ExecuteSearch);
            GoToUserProfileCommand = new MvxCommand(ShowCurrentUserProfile);

            _loadingStatusMessageToken = Messenger.Subscribe<LoadingStatusMessage>
                (message => IsLoading = message.IsLoadingIndicatorActive);
            _appBarHeaderChangeMessageToken = Messenger.Subscribe<AppBarHeaderChangeMessage>
                (message => PageHeader = message.HeaderTitle);
        }


        public override async void Start()
        {
            //HACK: Delay is added so that the MainViewModel can completely load first before showing other ViewModels.
            //Without the delay the ViewModels were not loading inside of the Frame in MainViewModel
            await Task.Delay(1000);

            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                ShowViewModel<EventsViewModel>();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }

            if (CrossSecureStorage.Current.HasKey("OAuthToken") && IsInternetAvailable())
            {
                User = await GithubClientService.GetAuthorizedGithubClient().User.Current();
            }
        }

        /// <summary>
        /// Navigate to ViewModel based on passed index from a listView
        /// </summary>
        /// <param name="index"></param>
        public void NavigateToViewModel(int index)
        {
            if (!CrossSecureStorage.Current.HasKey("OAuthToken"))
                return;
            switch (index)
            {
                case 0:
                    ShowViewModel<EventsViewModel>();
                    break;
                case 1:
                    ShowViewModel<NotificationsViewModel>();
                    break;
                case 2:
                    ShowViewModel<RepositoriesViewModel>();
                    break;
                case 3:
                    ShowViewModel<IssuesViewModel>();
                    break;
                case 4:
                    ShowViewModel<GistsViewModel>();
                    break;
                case 5:
                    ShowViewModel<SettingsViewModel>();
                    break;
            }
        }

        public void ShowEvents()
        {
            ShowViewModel<EventsViewModel>();
        }

        public void ShowLogin()
        {
            ShowViewModel<LoginViewModel>();
        }

        private void ShowSettings()
        {
            if (!CrossSecureStorage.Current.HasKey("OAuthToken"))
                return;
            ShowViewModel<SettingsViewModel>();
        }

        private void ShowCurrentUserProfile()
        {
            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                ShowViewModel<UserViewModel>();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }
        }

        public void GoToSearchViewModel(string searchterm)
        {
            if (!CrossSecureStorage.Current.HasKey("OAuthToken"))
                return;
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                ShowViewModel<SearchViewModel>(new
                {
                    searchTerm = searchterm,
                    searchType = SearchTypeEnumeration.Issues
                });
            }
        }

        private void ExecuteSearch()
        {
            if (!CrossSecureStorage.Current.HasKey("OAuthToken"))
                return;
            if (!string.IsNullOrWhiteSpace(SearchBoxText))
            {
                ShowViewModel<SearchViewModel>(new
                {
                    searchTerm = SearchBoxText,
                    searchType = SearchTypeEnumeration.Issues
                });
            }
        }
    }
}
