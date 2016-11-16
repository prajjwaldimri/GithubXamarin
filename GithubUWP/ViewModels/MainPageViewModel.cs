using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Services;
using Octokit;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Activity> FeedList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            //Initializing Octokit
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            await HelpingWorker.RoamingLoggedInKeyVerifier();
            var passwordCredential = HelpingWorker.VaultApiKeyRetriever();
            if (passwordCredential!=null)
            {
                client.Credentials = new Credentials(passwordCredential.Password);

                var userEvents =
                    await client.Activity.Events.GetAllUserReceived(client.User.Current().Result.Login);
                FeedList = userEvents.ToObservableCollection();
                //If in any case retrieves any unread notification remove it from the List.
            }
            RaisePropertyChanged(String.Empty);
        }

    }
}

