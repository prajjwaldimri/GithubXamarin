using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Views;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class RepositoryPageViewModel : ViewModelBase
    {
        public string RepositoryName { get; set; }
        public string RepositoryReadme { get; set; }
        public string RepositoryOwner { get; set; }
        public string RepositoryWebsite { get; set; }
        public string RepositoryDescription { get; set; }
        public int StarCount { get; set; }
        public int ForkCount { get; set; }


        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Busy.SetBusy(true,"Getting Details of Repository");
            var repository = SessionState.Get<Repository>(parameter.ToString());
            SessionState.Remove(parameter.ToString());
            RepositoryName = repository.FullName;
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
            RepositoryOwner = repository.Owner.Login;
            RepositoryWebsite = repository.Homepage;
            RepositoryDescription = repository.Description;
            StarCount = repository.StargazersCount;
            ForkCount = repository.ForksCount;
            RaisePropertyChanged(String.Empty);
            Busy.SetBusy(false);
        }
    }
}
