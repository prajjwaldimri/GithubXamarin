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
            RefreshGithubClients();
        }

        private void RefreshGithubClients()
        {
            UnAuthorizedGitHubClient = new GitHubClient(new ProductHeaderValue("githubuwp"));
            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                //Uses https://github.com/sameerkapps/SecureStorage
                var oAuthToken = CrossSecureStorage.Current.GetValue("OAuthToken");
                AuthorizedGithubClient = UnAuthorizedGitHubClient;
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
    }
}
