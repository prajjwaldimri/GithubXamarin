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
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Template10.Services.NavigationService;

namespace GithubUWP.ViewModels
{
    public class RepositoriesPageViewModel : ViewModelBase
    {
        private DelegateCommand<ItemClickEventArgs> _repositoryClickDelegateCommand;
        private DelegateCommand _pullToRefreshDelegateCommand;

        public string RepositoriesPageHeader { get; set; }
        public DelegateCommand<ItemClickEventArgs> RepositoryClickDelegateCommand
            =>
            _repositoryClickDelegateCommand ??
            (_repositoryClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(ExecuteNavigation));

        //PullToRefresh Command
        public DelegateCommand PullToRefreshDelegateCommand
            =>
                _pullToRefreshDelegateCommand ??
                (_pullToRefreshDelegateCommand = new DelegateCommand(RefreshList));

        
        /// <summary>
        /// Binds to the ListView on Repositories Page
        /// </summary>
        public ObservableCollection<Repository> RepositoriesList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Views.Busy.SetBusy(true, "Getting your repositories");
            await GetRepositories(parameter);
            Views.Busy.SetBusy(false, string.Empty);
        }

        private async Task GetRepositories(object parameter = null)
        {
            //Check for internet connectivity
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                var messageDialog = new MessageDialog("No Internet Connection!");
                await messageDialog.ShowAsync();
                return;
            }
            
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
                    var repository = SessionState.Get<Repository>(parameter.ToString());
                    var repoClient = new RepositoriesClient(new ApiConnection(new Connection(new ProductHeaderValue("githubuwp"))));
                    repositories = await repoClient.Forks.GetAll(repository.Id);
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
        }

        
        /// <summary>
        /// Navigates to the RepositoryPage when an Item is clicked on ListView
        /// </summary>
        /// <param name="itemClickEventArgs"></param>
        private async void ExecuteNavigation(ItemClickEventArgs itemClickEventArgs)
        {
            var clickedRepository = (Repository)itemClickEventArgs.ClickedItem;
            const string key = nameof(clickedRepository);
            if (SessionState.ContainsKey(key) == true)
            {
                SessionState.Remove(key);
            }
            SessionState.Add(key, clickedRepository);
            await NavigationService.NavigateAsync(typeof(Views.RepositoryPage), key);
        }

        private async void RefreshList()
        {
            Views.Busy.SetBusy(true,"Refreshing");
            await GetRepositories();
            Views.Busy.SetBusy(false,string.Empty);
        }
    }
}
