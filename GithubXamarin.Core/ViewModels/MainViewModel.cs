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
            get { return _searchBoxText;}
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
                User = await GithubClientService.GetAuthorizedGithubClient().User.Current();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }
        }

        /// <summary>
        /// Navigate to ViewModel based on passed index from a listView
        /// </summary>
        /// <param name="index"></param>
        private void NavigateToViewModel(int index)
        {
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
                    break;
                default:
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

        private void ExecuteSearch()
        {
            ShowViewModel<SearchViewModel>(new
            {
                searchTerm = SearchBoxText,
                searchType = SearchTypeEnumeration.Issues
            });
        }

        //Android Navigation Drawer
        public void ShowViewModelByNavigationDrawerMenuItem(int itemId)
        {
            switch (itemId)
            {
                case 0:
                    ShowViewModel<MainViewModel>();
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
                    //TODO: Gists ViewModel
                    break;
            }
        }
    }
}
