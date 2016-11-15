using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        public string UserName { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Initializing Octokit
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
            {
                if (vault.FindAllByResource("GithubAccessToken") != null)
                {
                    var passwordCredential = vault.Retrieve("GithubAccessToken", "Github");
                    client.Credentials = new Credentials(passwordCredential.Password);

                    //Parameter should be login
                    var user = await client.User.Get(parameter.ToString());
                    UserName = user.Name;
                    //If in any case retrieves any unread notification remove it from the List.
                }
            }
            RaisePropertyChanged(String.Empty);
        }
    }
}
