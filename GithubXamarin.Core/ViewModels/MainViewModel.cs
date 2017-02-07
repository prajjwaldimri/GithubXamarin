using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
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

        public MainViewModel(IGithubClientService githubClientService, IMvxMessenger messenger) : base(githubClientService, messenger)
        {
            PageHeader = "Main Page";
            HamburgerMenuNavigationCommand = new MvxCommand<int>(NavigateToViewModel);
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
