using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Services;
using Octokit;
using Template10.Mvvm;
using Template10.Services.PopupService;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class RepositoriesPageViewModel : ViewModelBase
    {
        private DelegateCommand<ItemClickEventArgs> _repositoryClickDelegateCommand;

        public string RepositoriesPageHeader { get; set; }
        public DelegateCommand<ItemClickEventArgs> RepositoryClickDelegateCommand
            =>
            _repositoryClickDelegateCommand ??
            (_repositoryClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(ExecuteNavigation));

        /// <summary>
        /// Binds to the ListView on Repositories Page
        /// </summary>
        public ObservableCollection<Repository> RepositoriesList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Views.Busy.SetBusy(true, "Getting your repositories");
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

                IReadOnlyList<Repository> repositories;
                if (parameter != null && SessionState.Get<Repository>(parameter.ToString()) != null)
                {
                    var repository= SessionState.Get<Repository>(parameter.ToString());
                    var repoClient = new RepositoriesClient(new ApiConnection(new Connection(new ProductHeaderValue("githubuwp"))));
                    repositories = await repoClient.Forks.GetAll(repository.Id);
                    SessionState.Remove(parameter.ToString());
                    RepositoriesPageHeader = $"Forks for {repository.FullName}";
                }
                else
                {
                    repositories = await client.Repository.GetAllForCurrent();
                    RepositoriesPageHeader = "Your Repositories";
                }
                RepositoriesList = repositories.ToObservableCollection();
            }


            RaisePropertyChanged(String.Empty);
            Views.Busy.SetBusy(false);
        }

        /// <summary>
        /// Navigates to the RepositoryPage when an Item is clicked on ListView
        /// </summary>
        /// <param name="itemClickEventArgs"></param>
        private async void ExecuteNavigation(ItemClickEventArgs itemClickEventArgs)
        {
            var clickedRepository = (Repository)itemClickEventArgs.ClickedItem;
            const string key = nameof(clickedRepository);
            SessionState.Add(key, clickedRepository);
            await NavigationService.NavigateAsync(typeof(Views.RepositoryPage), key);
        }
    }
}
