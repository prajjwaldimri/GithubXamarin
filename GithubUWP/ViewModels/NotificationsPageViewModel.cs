using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase
    {
        public ObservableCollection<Notification> NotificationList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Initializing Octokit
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            if (vault.FindAllByResource("GithubAccessToken") != null)
            {
                var passwordCredential = vault.Retrieve("GithubAccessToken", "Github");
                client.Credentials = new Credentials(passwordCredential.Password);

                var notifications = await client.Activity.Notifications.GetAllForCurrent();

                //If in any case retrieves any unread notification remove it from the List.
                foreach (var notification in notifications)
                {
                    if (notification.Unread)
                    {
                        NotificationList.Add(notification);
                    }
                }
            }
            
            RaisePropertyChanged(String.Empty);
        }
    }
}
