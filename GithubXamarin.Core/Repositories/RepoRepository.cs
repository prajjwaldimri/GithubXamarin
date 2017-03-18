using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/repos/
    /// </summary>
    public class RepoRepository : BaseRepository, IRepoRepository
    {
        private RepositoryForksClient _repositoryForksClient;
        private RepositoriesClient _repositoriesClient;

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
            if (_SearchClient == null)
            {
                _SearchClient = new SearchClient(new ApiConnection(githubClient.Connection));
            }
            var searchResult = await _SearchClient.SearchRepo(new SearchRepositoriesRequest(searchTerm));
            return searchResult.Items;
        }

        public async Task<Repository> ForkRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_repositoryForksClient == null)
            {
                _repositoryForksClient = new RepositoryForksClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _repositoryForksClient.Create(repositoryId, new NewRepositoryFork());
        }

        public async Task<bool> StarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGitHubClient)
        {
            if (_StarredClient == null)
            {
                _StarredClient = new StarredClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _StarredClient.StarRepo(repositoryOwner,repositoryName);
        }

        public async Task<bool> UnStarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGitHubClient)
        {
            if (_StarredClient == null)
            {
                _StarredClient = new StarredClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _StarredClient.RemoveStarFromRepo(repositoryOwner,repositoryName);
        }

        public async Task<Repository> CreateRepository(NewRepository newRepositoryDetails, GitHubClient authorizedGitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _repositoriesClient.Create(newRepositoryDetails);
        }

        public async Task<Repository> UpdateRepository(long repositoryId, RepositoryUpdate updatedRepositoryDetails, GitHubClient authorizedGitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _repositoriesClient.Edit(repositoryId, updatedRepositoryDetails);
        }
    }
}
