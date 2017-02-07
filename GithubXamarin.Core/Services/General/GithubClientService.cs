using GithubXamarin.Core.Contracts.Service;
using Octokit;
using Plugin.SecureStorage;


namespace GithubXamarin.Core.Services.General
{
    public class GithubClientService : IGithubClientService
    {
        public static GitHubClient UnAuthorizedGitHubClient;
        public static GitHubClient AuthorizedGithubClient;
        

        public GithubClientService()
        {
        }

        private void RefreshGithubClients()
        {
            UnAuthorizedGitHubClient = new GitHubClient(new ProductHeaderValue("gitit"));
            if (LoggedIn())
            {
                //Uses https://github.com/sameerkapps/SecureStorage
                AuthorizedGithubClient = UnAuthorizedGitHubClient;
                var oAuthToken = CrossSecureStorage.Current.GetValue("OAuthToken");
                AuthorizedGithubClient.Credentials = new Credentials(oAuthToken);
                return;
            }
            AuthorizedGithubClient = null;
        }


        public GitHubClient GetAuthorizedGithubClient()
        {
            if (AuthorizedGithubClient == null)
            {
                RefreshGithubClients();
            }
            return AuthorizedGithubClient;
        }

        public GitHubClient GetUnAuthorizedGithubClient()
        {
            if (UnAuthorizedGitHubClient == null)
            {
                RefreshGithubClients();
            }
            return UnAuthorizedGitHubClient;
        }

        public bool LoggedIn()
        {
            return CrossSecureStorage.Current.HasKey("OAuthToken");
        }

        public void SaveGithubOAuthToken(string token)
        {
            if (!LoggedIn())
                CrossSecureStorage.Current.SetValue("OAuthToken", token);
        }
    }
}
