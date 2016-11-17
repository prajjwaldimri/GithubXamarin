using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using GithubUWP.Services;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class UserProfilePageViewModel : ViewModelBase
    {
        //User-Data Variables
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string ProfileUrl { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
        public ImageSource AvatarImage { get; set; }

        //User Stats
        public string Followers { get; set; }
        public string Following { get; set; }
        public string DiskUsage { get; set; }
        public string CreatedAt { get; set; }
        public string Hireable { get; set; }
        public string TotalGists { get; set; }
        public string PrivateRepos { get; set; }
        public string PublicRepos { get; set; }
        public string AccountPlan { get; set; }
        public string AccountType { get; set; }

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
            await HelpingWorker.RoamingLoggedInKeyVerifier();
            var passwordCredential = HelpingWorker.VaultApiKeyRetriever();
            if (passwordCredential != null)
            {
                client.Credentials = new Credentials(passwordCredential.Password);

                var currentUser = await client.User.Current();
                DisplayName = currentUser.Name;
                UserName = currentUser.Login;
                Location = currentUser.Location;
                Email = currentUser.Email;
                ProfileUrl = currentUser.HtmlUrl;
                Bio = currentUser.Bio ?? "No Bio Found";
                Company = currentUser.Company ?? "No Company Assigned";
                AvatarImage = new BitmapImage(new Uri(currentUser.AvatarUrl, UriKind.RelativeOrAbsolute));
                //Raises property changed event for all the properties.
                RaisePropertyChanged(string.Empty);

                Followers = currentUser.Followers.ToString();
                Following = currentUser.Following.ToString();
                DiskUsage = currentUser.DiskUsage.ToString();
                CreatedAt = currentUser.CreatedAt.ToString();
                Hireable = currentUser.Hireable.ToString();
                TotalGists = (currentUser.PrivateGists + currentUser.PublicGists).ToString();
                PrivateRepos = currentUser.TotalPrivateRepos.ToString();
                PublicRepos = currentUser.PublicRepos.ToString();
                AccountPlan = currentUser.Plan.Name;
                AccountType = currentUser.Type.ToString();
            }

            RaisePropertyChanged(string.Empty);
        }
    }
}
