using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using Octokit;
using Plugin.SecureStorage;

namespace GithubXamarin.Core.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        #region Properties and Commands

        public IEnumerable<string> MenuItems { get; private set; } = new[] { "Option1", "Option2" };

        private readonly IUpdateService _updateService;
        private readonly IFileDataService _fileDataService;

        private string _pageHeader;
        public string PageHeader
        {
            get => _pageHeader;
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
            get => _isLoading;
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        private string _searchBoxText;
        public string SearchBoxText
        {
            get => _searchBoxText;
            set { _searchBoxText = value; RaisePropertyChanged(() => SearchBoxText); }
        }

        private User _user;
        public User User
        {
            get => _user;
            set { _user = value; RaisePropertyChanged(() => User); }
        }

        private readonly MvxSubscriptionToken _loadingStatusMessageToken;
        private readonly MvxSubscriptionToken _appBarHeaderChangeMessageToken;

        #endregion

        public MainViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IUpdateService updateService, IFileDataService fileDataService) : base(githubClientService, messenger, dialogService)
        {
            HamburgerMenuNavigationCommand = new MvxCommand<int>(NavigateToViewModel);
            GoToSettingsCommand = new MvxCommand(ShowSettings);
            SearchCommand = new MvxCommand(ExecuteSearch);
            GoToUserProfileCommand = new MvxCommand(ShowCurrentUserProfile);

            _loadingStatusMessageToken = Messenger.Subscribe<LoadingStatusMessage>
                (message => IsLoading = message.IsLoadingIndicatorActive);
            _appBarHeaderChangeMessageToken = Messenger.Subscribe<AppBarHeaderChangeMessage>
                (message => PageHeader = message.HeaderTitle);

            _updateService = updateService;
            _fileDataService = fileDataService;
        }

        /// <exception cref="HttpRequestException">On line 103</exception>
        public async Task LoadFragments()
        {
            //HACK: This delay is required to give the main layout a little bit of time to completely load
            await Task.Delay(10);

            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                ShowViewModel<EventsViewModel>();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }

            if (CrossSecureStorage.Current.HasKey("OAuthToken") && await IsInternetAvailable())
            {
                try
                {
                    User = await GithubClientService.GetAuthorizedGithubClient().User.Current();
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine(e);
                }
            }
            await CheckIfUpdated();
        }

        /// <summary>
        /// Checks if the app is updated
        /// </summary>
        /// <returns></returns>
        private async Task CheckIfUpdated()
        {
            if (_updateService.IsAppUpdated())
            {
                var releaseNotes = await _fileDataService.GetFile(73414278, "RELEASE_NOTES", GithubClientService.GetAuthorizedGithubClient());
                await DialogService.ShowMarkdownDialogAsync(releaseNotes.Content);
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
