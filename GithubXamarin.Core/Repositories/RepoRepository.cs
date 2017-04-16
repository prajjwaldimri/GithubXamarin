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
        private WatchedClient _watchedClient;

        public async Task<Repository> GetRepository(long repositoryId, GitHubClient githubClient)
        {
            return await githubClient.Repository.Get(repositoryId);
        }

        public async Task<IEnumerable<Repository>> GetAllRepositoriesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Repository.GetAllForCurrent();
        }

        public async Task<IEnumerable<Repository>> GetAllStarredRepositoriesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Activity.Starring.GetAllForCurrent();
        }

        public async Task<IEnumerable<Repository>> GetAllRepositoriesForUser(string login, GitHubClient gitHubClient)
        {
            return await gitHubClient.Repository.GetAllForUser(login);
        }

        public async Task<IEnumerable<Repository>> GetAllStarredRepositoriesForUser(string login, GitHubClient gitHubClient)
        {
            return await gitHubClient.Activity.Starring.GetAllForUser(login);
        }

        public async Task<IEnumerable<RepositoryContent>> GetContentsOfRepository(long repoId,
            GitHubClient authorizedGitHubClient, string path = null)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                return await _repositoriesClient.Content.GetAllContents(repoId);
            }
            return await _repositoriesClient.Content.GetAllContents(repoId, path);
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
            return await _StarredClient.StarRepo(repositoryOwner, repositoryName);
        }

        public async Task<bool> UnStarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGitHubClient)
        {
            if (_StarredClient == null)
            {
                _StarredClient = new StarredClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _StarredClient.RemoveStarFromRepo(repositoryOwner, repositoryName);
        }

        public async Task<Subscription> WatchRepository(long repositoryId, NewSubscription subscription, GitHubClient authorizedGitHubClient)
        {
            if (_watchedClient == null)
            {
                _watchedClient = new WatchedClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _watchedClient.WatchRepo(repositoryId, subscription);
        }

        public async Task<bool> UnWatchRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_watchedClient == null)
            {
                _watchedClient = new WatchedClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _watchedClient.UnwatchRepo(repositoryId);
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

        public async Task DeleteRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            await _repositoriesClient.Delete(repositoryId);
        }
    }
}
