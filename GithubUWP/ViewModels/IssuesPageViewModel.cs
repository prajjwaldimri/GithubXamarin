using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class IssuesPageViewModel : ViewModelBase
    {
        public ObservableCollection<Issue> IssuesList { get; set; }

        private DelegateCommand<ItemClickEventArgs> _issueClickDelegateCommand;

        public DelegateCommand<ItemClickEventArgs> IssueClickDelegateCommand
            => _issueClickDelegateCommand ?? (_issueClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(ExecuteNavigation));
        
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var client = new GitHubClient(new ProductHeaderValue("githubuwp"));
            var vault = new PasswordVault();
            if (vault.FindAllByResource("GithubAccessToken") != null)
            {
               var passwordCredential = vault.Retrieve("GithubAccessToken", "Github");
                client.Credentials = new Credentials(passwordCredential.Password);

                var issues = await client.Issue.GetAllForCurrent();
                IssuesList = issues.ToObservableCollection();
                
            }
            RaisePropertyChanged(String.Empty);
        }

        private async void ExecuteNavigation(ItemClickEventArgs obj)
        {
            var issue = (Issue) obj.ClickedItem;
            const string key = nameof(issue);
            SessionState.Add(key, issue);
            await NavigationService.NavigateAsync(typeof(Views.IssuePage),key);
        }

    }
}
