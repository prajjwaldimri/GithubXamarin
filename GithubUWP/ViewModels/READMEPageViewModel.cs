using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class READMEPageViewModel : ViewModelBase
    {
        public string READMEContent { get; set; }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
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
            var repository = SessionState.Get<Repository>(parameter.ToString());
            var repoContent = new RepositoryContentsClient(new ApiConnection(client.Connection));
            READMEContent = (await repoContent.GetReadme(repository.Id)).Content; 
            RaisePropertyChanged(string.Empty);
            SessionState.Remove(parameter.ToString());
        }
    }
}
