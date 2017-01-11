using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/repos/
    /// </summary>
    public class RepoRepository : IRepoRepository
    {

        public async Task<Repository> GetRepository(long repositoryId, GitHubClient githubClient)
        {
            return await githubClient.Repository.Get(repositoryId);
        }

        public async Task<IEnumerable<Repository>> GetAllRepositoriesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Repository.GetAllForCurrent();
        }

        public async Task<IEnumerable<Repository>> GetAllRepositoriesForUser(string login, GitHubClient gitHubClient)
        {
            return await gitHubClient.Repository.GetAllForUser(login);
        }

        public async Task<IEnumerable<Repository>> SearchRepositories(string searchTerm, GitHubClient githubClient)
        {
            var searchClient = new SearchClient(new ApiConnection(githubClient.Connection));
            var searchResult = await searchClient.SearchRepo(new SearchRepositoriesRequest(searchTerm));
            return searchResult.Items;
        }

        public async Task<Repository> ForkRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            var repoForkClient = new RepositoryForksClient(new ApiConnection(authorizedGitHubClient.Connection));
            return await repoForkClient.Create(repositoryId, new NewRepositoryFork());
        }

        public async Task<bool> StarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGitHubClient)
        {
            var starredClient = new StarredClient(new ApiConnection(authorizedGitHubClient.Connection));
            return await starredClient.StarRepo(repositoryOwner,repositoryName);
        }

        public async Task<bool> UnStarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGitHubClient)
        {
            var starredClient = new StarredClient(new ApiConnection(authorizedGitHubClient.Connection));
            return await starredClient.RemoveStarFromRepo(repositoryOwner,repositoryName);
        }
    }
}
