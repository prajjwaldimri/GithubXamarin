using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using GithubXamarin.UWP.Services;

namespace GithubXamarin.UWP.ViewModels
{
    public class NotificationsPageViewModel :  ViewModelBase
    {
        private DelegateCommand _pullToReDelegateCommand;

        public ObservableCollection<Notification> NotificationList { get; set; }

        public DelegateCommand PullToRefreshDelegateCommand
            => _pullToReDelegateCommand ?? (_pullToReDelegateCommand = new DelegateCommand(Refresh));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Check for internet connectivity
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                var messageDialog = new MessageDialog("No Internet Connection!");
                await messageDialog.ShowAsync();
                return;
            }
            Views.Busy.SetBusy(true,"Getting your notifications...");
            await GetNotifications();
            Views.Busy.SetBusy(false);
        }

        private async Task GetNotifications()
        {
            GitHubClient client;
            //Check to see if a client already exists in the session and if not then create a new one and add it to the current session.
            if (SessionState.Get<GitHubClient>("GitHubClient") != null)
            {
                client = SessionState.Get<GitHubClient>("GitHubClient");
            }
            else
            {
                client = new GitHubClient(new ProductHeaderValue("githubuwp"));
                SessionState.Add("GitHubClient", client);
            }
            await HelpingWorker.RoamingLoggedInKeyVerifier();
            var passwordCredential = HelpingWorker.VaultAccessTokenRetriever();
            if (passwordCredential != null)
            {
                client.Credentials = new Credentials(passwordCredential.Password);

                var notifications = await client.Activity.Notifications.GetAllForCurrent();
                NotificationList = notifications.ToObservableCollection();
            }
            RaisePropertyChanged(String.Empty);
        }

        private async void Refresh()
        {
            await GetNotifications();
        }
    }
}
