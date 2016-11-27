using System;
using GithubUWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;
using GithubUWP.UserControls;
using System.Threading.Tasks;
using Octokit;

namespace GithubUWP.Views
{
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        GitHubClient client;
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private async void PullToRefreshExtender_RefreshRequested(object sender, AmazingPullToRefresh.Controls.RefreshRequestedEventArgs e)
        {
            var userEvents =
                    await client.Activity.Events.GetAllUserReceived(client.User.Current().Result.Login);

            var deferral = e.GetDeferral();
            await Task.Delay(2500); // something
            deferral.Complete();
        }
    }
}
