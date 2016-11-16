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

namespace GithubUWP.ViewModels
{
    public class GistsPageViewModel : ViewModelBase
    {

        public ObservableCollection<Gist> GistList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Initializing Octokit
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            await HelpingWorker.RoamingLoggedInKeyVerifier();
            var passwordCredential = HelpingWorker.VaultApiKeyRetriever();
            if (passwordCredential != null)
            {
                client.Credentials = new Credentials(passwordCredential.Password);
                var gists = await client.Gist.GetAllForUser(client.User.Current().Result.Login);
                GistList = gists.ToObservableCollection();
            }
            //If in any case retrieves any unread notification remove it from the List.
            RaisePropertyChanged(String.Empty);
        }
    }
}
