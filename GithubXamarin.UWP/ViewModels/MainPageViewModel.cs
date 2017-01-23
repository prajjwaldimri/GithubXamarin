using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using GithubXamarin.UWP.Services;
using Octokit;
using Template10.Utils;
using Windows.Security.Credentials;
using Windows.ApplicationModel.Background;

namespace GithubXamarin.UWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private DelegateCommand<TappedRoutedEventArgs> _activityClickDelegateCommand;
        private DelegateCommand _pullToRefreshDelegateCommand;

        public DelegateCommand<TappedRoutedEventArgs> ActivityDelegateCommand
            =>
            _activityClickDelegateCommand ?? (_activityClickDelegateCommand = new DelegateCommand<TappedRoutedEventArgs>(ExecuteNavigation));

        public DelegateCommand PullToRefreshDelegateCommand
            => _pullToRefreshDelegateCommand ?? (_pullToRefreshDelegateCommand = new DelegateCommand(Refresh));

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
            await GetActivities();
            await RegisterBackgroundTask();
            Views.Busy.SetBusy(false);
        }

        private async Task GetActivities()
        {
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
            if (passwordCredential != null)
            {
                client.Credentials = new Credentials(passwordCredential.Password);

                var userEvents =
                    await client.Activity.Events.GetAllUserReceived(client.User.Current().Result.Login);
                FeedList = userEvents.ToObservableCollection();
                //If in any case retrieves any unread notification remove it from the List.
            }
            RaisePropertyChanged(String.Empty);
        }

        private void ExecuteNavigation(TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            //Have to handle the navigation based on the type of Object
        }

        private async void Refresh()
        {
            Views.Busy.SetBusy(true,"Refreshing");
            await GetActivities();
            Views.Busy.SetBusy(false);
        }

        private async Task RegisterBackgroundTask()
        {
            //Register GithubNotificationsBackgroundTask
            var taskRegistered = false;
            var taskName = "GithubNotificationsBackgroundTask";

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    task.Value.Unregister(true);
                    taskRegistered = true;
                    break;
                }
            }

            if (!taskRegistered)
            {
                var builder = new BackgroundTaskBuilder();

                var access = await BackgroundExecutionManager.RequestAccessAsync();
                switch (access)
                {
                    case BackgroundAccessStatus.DeniedByUser:
                        break;
                    case BackgroundAccessStatus.DeniedBySystemPolicy:
                        break;
                    case BackgroundAccessStatus.Unspecified:
                        break;
                    default:
                        builder.Name = taskName;
                        builder.SetTrigger(new TimeTrigger(15, false));
                        builder.IsNetworkRequested = true;
                        builder.TaskEntryPoint =
                            typeof(Background.GithubNotificationsBackgroundTask).FullName;
                        var task = builder.Register();
                        break;
                }
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        taskRegistered = true;
                        break;
                    }
                }
            }
        }
    }
}

