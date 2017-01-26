using System;
using System.Net.Http;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using Octokit;
using Plugin.SecureStorage;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;


namespace GithubXamarin.Core.Services.General
{
    public class GithubClientService : IGithubClientService
    {
        public static GitHubClient UnAuthorizedGitHubClient;
        public static GitHubClient AuthorizedGithubClient;
        private SecureKeysRetrievingService _secureKeysRetrievingService;

        public GithubClientService()
        {
        }

        private void RefreshGithubClients()
        {
            UnAuthorizedGitHubClient = new GitHubClient(new ProductHeaderValue("gitit"));
            if (LoggedIn())
            {
                //Uses https://github.com/sameerkapps/SecureStorage
                string oAuthToken = null;
                AuthorizedGithubClient = UnAuthorizedGitHubClient;
                oAuthToken = CrossSecureStorage.Current.GetValue("OAuthToken");
                AuthorizedGithubClient.Credentials = new Credentials(oAuthToken);
                return;
            }
            AuthorizedGithubClient = null;
        }


        public GitHubClient GetAuthorizedGithubClient()
        {
            if (AuthorizedGithubClient == null)
                RefreshGithubClients();
            return AuthorizedGithubClient;
        }

        public GitHubClient GetUnAuthorizedGithubClient()
        {
            if (UnAuthorizedGitHubClient == null)
                RefreshGithubClients();
            return UnAuthorizedGitHubClient;
        }

        public bool LoggedIn()
        {
            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                return true;
            }
            return false;
        }

        public void SaveGithubOAuthToken(string token)
        {
            if (!LoggedIn())
                CrossSecureStorage.Current.SetValue("OAuthToken", token);
        }
    }
}
