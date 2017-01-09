using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

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
            //TODO: Get OAuth using secure mechanisms

            UnAuthorizedGitHubClient = new GitHubClient(new ProductHeaderValue("githubuwp"));
            AuthorizedGithubClient = UnAuthorizedGitHubClient;
            AuthorizedGithubClient.Credentials = new Credentials("OAuth token");
        }
    }
}
