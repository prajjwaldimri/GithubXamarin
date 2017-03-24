using Android.OS;
using Android.Webkit;
using Android.Runtime;
using Android.Views;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using Octokit;
using System;
using GithubXamarin.Droid.Services;
using Plugin.SecureStorage;

namespace GithubXamarin.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("githubxamarin.droid.views.LoginFragment")]
    public class LoginFragment : MvxFragment<LoginViewModel>
    {
        private GitHubClient _client;
        private string _clientId;
        private string _clientSecret;
        private WebView _webView;

        public new LoginViewModel ViewModel
        {
            get { return (LoginViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.LoginView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            ApiKeysManager.KeyRetriever();
            _webView = view.FindViewById<WebView>(Resource.Id.login_webview);
            _webView.Settings.JavaScriptEnabled = true;
            //Values can be found at https://github.com/settings/applications
            _clientId = ApiKeysManager.GithubClientId;
            _clientSecret = ApiKeysManager.GithubClientSecret;
            _client = new GitHubClient(new ProductHeaderValue("gitit"));

            _webView.SetWebViewClient(new LoginWebViewClient(_client,_clientId,_clientSecret,_webView));

            var loginRequest = new OauthLoginRequest(_clientId)
            {
                Scopes = { "user", "notifications", "repo", "delete_repo", "gist", "admin:org" }
            };

            var oAuthLoginUrl = _client.Oauth.GetGitHubLoginUrl(loginRequest);
            _webView.LoadUrl(oAuthLoginUrl.ToString());
            
        }

        }

    public class LoginWebViewClient : WebViewClient
    {
        private static GitHubClient _client;
        private static string _clientId;
        private static string _clientSecret;
        private static WebView _webView;

        //Have to pass these parameters from the main fragment.
        public LoginWebViewClient(GitHubClient client, string clientId, string clientSecret, WebView webview)
        {
            _client = client;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _webView = webview;
        }

        /// <summary>
        /// Gets Fired whenever a page in the WebView finishes loading.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="url"></param>
        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            if (url.Contains("code="))
            {
                var code = url.Split(new[] { "code=" }, StringSplitOptions.None)[1];
                code = code.Replace("&state=", string.Empty);
                var tokenRequest = new OauthTokenRequest(_clientId, _clientSecret, code);
                var accessToken = _client.Oauth.CreateAccessToken(tokenRequest).Result;
                CrossSecureStorage.Current.SetValue("OAuthToken", accessToken.AccessToken.ToString());
            }
        }
    }
}