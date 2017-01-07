using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Services;
using Octokit;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Popups;
using System.Net.NetworkInformation;

namespace GithubUWP.ViewModels
{
    public class IssuesPageViewModel : ViewModelBase
    {
        private object _parameter;
        private DelegateCommand<ItemClickEventArgs> _issueClickDelegateCommand;
        private DelegateCommand _pullToRefreshDelegateCommand;

        public DelegateCommand<ItemClickEventArgs> IssueClickDelegateCommand
            => _issueClickDelegateCommand ?? (_issueClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(ExecuteNavigation));

        public DelegateCommand PullToRefreshDelegateCommand
            => _pullToRefreshDelegateCommand ?? (_pullToRefreshDelegateCommand = new DelegateCommand(Refresh));

        public string IssuesHeader { get; set; }

        public ObservableCollection<Issue> IssuesList { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Check for internet connectivity
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                var messageDialog = new MessageDialog("No Internet Connection!");
                await messageDialog.ShowAsync();
                return;
            }
            Views.Busy.SetBusy(true, "Getting your issues");
            _parameter = parameter;
            await GetIssues();
            Views.Busy.SetBusy(false);
        }

        private async void ExecuteNavigation(ItemClickEventArgs obj)
        {
            var issue = (Issue)obj.ClickedItem;
            const string key = nameof(issue);
            if (SessionState.ContainsKey(key) == true)
            {
                SessionState.Remove(key);
            }
            SessionState.Add(key, issue);
            await NavigationService.NavigateAsync(typeof(Views.IssuePage), key);
        }

        private async Task GetIssues()
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
            await HelpingWorker.RoamingLoggedInKeyVerifier();
            var passwordCredential = HelpingWorker.VaultAccessTokenRetriever();
            if (passwordCredential != null)
            {
                client.Credentials = new Credentials(passwordCredential.Password);
                IReadOnlyList<Issue> issues;
                //Checks if the request for Issues page came from hamburger menu or from other page.
                if (_parameter != null && SessionState.Get<Repository>(_parameter.ToString()) != null)
                {
                    var repository = SessionState.Get<Repository>(_parameter.ToString());
                    var issuesClient = new IssuesClient(new ApiConnection(client.Connection));
                    issues = await issuesClient.GetAllForRepository(repository.Id);
                    IssuesHeader = $"Issues in {repository.Name}";
                    SessionState.Remove(_parameter.ToString());
                }
                else
                {
                    issues = await client.Issue.GetAllForCurrent();
                    IssuesHeader = "Issues";
                }
                IssuesList = issues.ToObservableCollection();
            }

            RaisePropertyChanged(String.Empty);
        }

        private async void Refresh()
        {
            await GetIssues();
        }

    }
}
