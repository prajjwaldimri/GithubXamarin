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
    public class GistsPageViewModel : ViewModelBase
    {

        public ObservableCollection<Gist> GistList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Check for internet connectivity
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                var messageDialog = new MessageDialog("No Internet Connection!");
                await messageDialog.ShowAsync();
                return;
            }
            Views.Busy.SetBusy(true, "Gettings Gists");
            //Initializing Octokit
            GitHubClient client;
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
                var gists = await client.Gist.GetAllForUser(client.User.Current().Result.Login);
                GistList = gists.ToObservableCollection();
            }
            //If in any case retrieves any unread notification remove it from the List.
            RaisePropertyChanged(String.Empty);
            Views.Busy.SetBusy(false);
        }
    }
}
