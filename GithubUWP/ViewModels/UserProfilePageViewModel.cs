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
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class UserProfilePageViewModel : ViewModelBase
    {
        public UserProfilePageViewModel()
        {
            
        }

        //User-Data Variables
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string ProfileUrl { get; set; }
        public string Bio { get; set; }
        public ImageSource AvatarImage { get; set; }

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

            var currentUser = await client.User.Current();
            DisplayName = currentUser.Name;
            UserName = currentUser.Login;
            Location = currentUser.Location;
            Email = currentUser.Email;
            ProfileUrl = currentUser.HtmlUrl;
            Bio = currentUser.Bio;
            AvatarImage = new BitmapImage(new Uri(currentUser.AvatarUrl, UriKind.RelativeOrAbsolute));

            //Raises property changed event for all the properties.
            RaisePropertyChanged(string.Empty);
        }
    }
}
