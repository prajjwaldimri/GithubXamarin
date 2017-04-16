using System.Collections.Generic;
using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IRepoRepository
    {
        Task<Octokit.Repository> GetRepository(long repositoryId, GitHubClient githubClient);

        Task<IEnumerable<Octokit.Repository>> GetAllRepositoriesForCurrentUser(GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Octokit.Repository>> GetAllStarredRepositoriesForCurrentUser(GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Octokit.Repository>> GetAllRepositoriesForUser(string login, GitHubClient gitHubClient);

        Task<IEnumerable<Octokit.Repository>> GetAllStarredRepositoriesForUser(string login, GitHubClient gitHubClient);

        Task<IEnumerable<RepositoryContent>> GetContentsOfRepository(long repoId, GitHubClient authorizedGitHubClient,
            string path = null);

        Task<IEnumerable<Octokit.Repository>> SearchRepositories(string searchTerm, GitHubClient githubClient);

        Task<Octokit.Repository> ForkRepository(long repositoryId, GitHubClient authorizedGithubClient);

        Task<bool> StarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGithubClient);

        Task<bool> UnStarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGithubClient);

        Task<Subscription> WatchRepository(long repositoryId, NewSubscription subscription,
            GitHubClient authorizedGitHubClient);

        Task<bool> UnWatchRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<Octokit.Repository> CreateRepository(NewRepository newRepositoryDetails, GitHubClient authorizedGitHubClient);

        Task<Octokit.Repository> UpdateRepository(long repositoryId, RepositoryUpdate updatedRepositoryDetails,
            GitHubClient authorizedGitHubClient);

        Task DeleteRepository(long repositoryId, GitHubClient authorizedGitHubClient);

    }
}