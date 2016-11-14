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
    public class RepositoriesPageViewModel : ViewModelBase
    {

        public ObservableCollection<Repository> RepositoriesList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            var passwordCredential = new PasswordCredential();
            if (vault.FindAllByResource("GithubAccessToken") != null)
            {
                passwordCredential = vault.Retrieve("GithubAccessToken", "Github");
            }
            client.Credentials = new Credentials(passwordCredential.Password);

            var repositories = await client.Repository.GetAllForCurrent();
            RepositoriesList = repositories.ToObservableCollection();
            RaisePropertyChanged(String.Empty);
        }
    }
}
