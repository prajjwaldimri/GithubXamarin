using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Common;
using Template10.Services.NavigationService;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private GitHubClient _client;
        private string _clientId;
        private string _clientSecret;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Values can be found at https://github.com/settings/applications
                _clientId = "";
                _clientSecret = "";
                _client = new GitHubClient(new ProductHeaderValue("githubuwp"));

                var loginRequest = new OauthLoginRequest(_clientId)
                {
                    Scopes = { "user", "notifications" }
                };

                var oAuthLoginUrl = _client.Oauth.GetGitHubLoginUrl(loginRequest);
                LoginWebView.Navigate(oAuthLoginUrl);
                LoginWebView.NavigationCompleted += LoginWebViewOnNavigationCompleted;
            
        }

        /// <summary>
        /// Method attached to navigation of the web-browser. It must return void
        /// TODO: async and void don't go well together. Should be Task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void LoginWebViewOnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.Uri  != null && args.Uri.ToString().Contains("code="))
            {
                await CodeRetrieverandTokenSaver(args.Uri.ToString());
            }
        }

        /// <summary>
        /// Retrieves code value from a given string and uses it to create access token and then saves it.
        /// </summary>
        /// <param name="retrievedUrl">Url from browser</param>
        /// <returns></returns>
        private async Task CodeRetrieverandTokenSaver(string retrievedUrl)
        {
            //Retrieves code from URL
            var code = retrievedUrl.Split(new[] {"code="}, StringSplitOptions.None)[1];
            var tokenRequest = new OauthTokenRequest(_clientId, _clientSecret, code);
            var accessToken = await _client.Oauth.CreateAccessToken(tokenRequest);

            //Storing Access Token in Credential Locker
            //More details: https://msdn.microsoft.com/en-us/windows/uwp/security/credential-locker
            var vault = new Windows.Security.Credentials.PasswordVault();
            if (vault.FindAllByResource("GithubAccessToken") != null)
            {
                var vaultItems = vault.RetrieveAll();
                foreach (var passwordCredential in vaultItems)
                {
                    vault.Remove(passwordCredential);
                }
            }
            vault.Add(new PasswordCredential("GithubAccessToken", "Github", accessToken.AccessToken));

            if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
                ApplicationData.Current.RoamingSettings.Values.Add("IsLoggedIn",true);
        }
    }
}
