using Octokit;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IGithubClientService
    {
        GitHubClient GetAuthorizedGithubClient();
        GitHubClient GetUnAuthorizedGithubClient();
    }
}