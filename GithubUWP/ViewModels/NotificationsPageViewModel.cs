using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Services;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;
using System.Net.NetworkInformation;
using Windows.UI.Popups;

namespace GithubUWP.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase
    {
        public ObservableCollection<Notification> NotificationList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Check for internet connectivity
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                var messageDialog = new MessageDialog("No Internet Connection!");
                await messageDialog.ShowAsync();
                return;
            }
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
    }
}
