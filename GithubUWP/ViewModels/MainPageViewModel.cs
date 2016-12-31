using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Services;
using Octokit;
using Template10.Common;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private DelegateCommand<TappedRoutedEventArgs> _activityClickDelegateCommand;

        public DelegateCommand<TappedRoutedEventArgs> ActivityDelegateCommand
            =>
            _activityClickDelegateCommand ?? (_activityClickDelegateCommand = new DelegateCommand<TappedRoutedEventArgs>(ExecuteNavigation))
        ;

        public ObservableCollection<Activity> FeedList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            //Check for internet connectivity
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                var messageDialog = new MessageDialog("No Internet Connection!");
                await messageDialog.ShowAsync();
                return;
            }
            //Initializing Octokit
            Views.Busy.SetBusy(true,"Getting your activities");
            GitHubClient client;
            try
            {
                if (SessionState.Get<GitHubClient>("GitHubClient") != null)
                {
                    client = SessionState.Get<GitHubClient>("GitHubClient");
                }
                else
                {
                    client = new GitHubClient(new ProductHeaderValue("githubuwp"));
                    SessionState.Add("GitHubClient", client);
                }
            }
            catch (Exception)
            {
                client = new GitHubClient(new ProductHeaderValue("githubuwp"));
                SessionState.Add("GitHubClient", client);
            }
            await HelpingWorker.RoamingLoggedInKeyVerifier();
            var passwordCredential = HelpingWorker.VaultAccessTokenRetriever();
            if (passwordCredential!=null)
            {
                client.Credentials = new Credentials(passwordCredential.Password);

                var userEvents =
                    await client.Activity.Events.GetAllUserReceived(client.User.Current().Result.Login);
                FeedList = userEvents.ToObservableCollection();
                //If in any case retrieves any unread notification remove it from the List.
            }
            RaisePropertyChanged(String.Empty);
            Views.Busy.SetBusy(false);
        }

        private void ExecuteNavigation(TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            //Have to handle the navigation based on the type of Object
        }
    }
}

