using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;

namespace GithubUWP.ViewModels
{
    public class RepositoriesPageViewModel : ViewModelBase
    {
        public object ClickedItem { get; set; }
        private DelegateCommand<ItemClickEventArgs> _repositoryClickDelegateCommand;

        public DelegateCommand<ItemClickEventArgs> RepositoryClickDelegateCommand
            =>
            _repositoryClickDelegateCommand ??
            (_repositoryClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(ExecuteNavigation));

        public ObservableCollection<Repository> RepositoriesList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            if (vault.FindAllByResource("GithubAccessToken") != null)
            {
                var passwordCredential = vault.Retrieve("GithubAccessToken", "Github");
                client.Credentials = new Credentials(passwordCredential.Password);

                var repositories = await client.Repository.GetAllForCurrent();
                RepositoriesList = repositories.ToObservableCollection();
            }
            
            RaisePropertyChanged(String.Empty);
        }

        private void ExecuteNavigation(ItemClickEventArgs itemClickEventArgs)
        {
            var clickedRepository = (Repository) itemClickEventArgs.ClickedItem;
            var key = nameof(clickedRepository);
            SessionState.Add(key, clickedRepository);
            NavigationService.Navigate(typeof(Views.RepositoryPage), key);
        }
    }
}
