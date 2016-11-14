using System.ComponentModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;
using Page = Windows.UI.Xaml.Controls.Page;

namespace GithubUWP.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        Services.SettingsServices.SettingsService _settings;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
            _settings = Services.SettingsServices.SettingsService.Instance;
            LoginProfileUpdater();
        }

        public void LoginProfileUpdater()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
            {
                LoginButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Visible;
            }
            else
            {
                LoginButton.Visibility = Visibility.Visible;
                ProfileButton.Visibility = Visibility.Collapsed;
            }
        }
       

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public async void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
            HamburgerMenu.RefreshStyles(_settings.AppTheme, true);
            HamburgerMenu.IsFullScreen = _settings.IsFullScreen;
            HamburgerMenu.HamburgerButtonVisibility = _settings.ShowHamburgerButton ? Visibility.Visible : Visibility.Collapsed;
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
                await ProfileImageandNotificationsSetter();
        }

        public async Task ProfileImageandNotificationsSetter()
        {
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            var passwordCredential = new PasswordCredential();
            if (vault.FindAllByResource("GithubAccessToken") != null)
            {
                passwordCredential = vault.Retrieve("GithubAccessToken", "Github");
            }
            client.Credentials = new Credentials(passwordCredential.Password);

            var notifications = await client.Activity.Notifications.GetAllForCurrent();
            NotificationIcon.Glyph = "\uf132";

            foreach (var notification in notifications)
            {
                if (!notification.Unread) continue;
                NotificationIcon.Glyph = "\uf3c3";
                break;
            }
            

            var currentUser = await client.User.Current();

            
            ProfileButtonImage.ImageSource = new BitmapImage(new Uri(currentUser.AvatarUrl));
            ProfileNameText.Text = currentUser.Name;
        }
    }
}

