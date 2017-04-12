using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/users/
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        private RepoCollaboratorsClient _repoCollaboratorsClient;
        private RepositoriesClient _repositoriesClient;
        private UsersClient _usersClient;

        public async Task<User> GetUser(string userLoginId, GitHubClient gitHubClient)
        {
            if (_usersClient == null)
            {
                _usersClient = new UsersClient(new ApiConnection(gitHubClient.Connection));
            }
            return await _usersClient.Get(userLoginId);
        }

        /// <summary>
        /// Gets all the details for the current logged in user.
        /// </summary>
        /// <param name="authorizedGitHubClient"></param>
        /// <returns></returns>
        public async Task<User> GetCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.User.Current();
        }

        public async Task<IEnumerable<User>> SearchUsers(string searchTerm, GitHubClient gitHubClient)
        {
            if (_SearchClient == null)
            {
                _SearchClient = new SearchClient(new ApiConnection(gitHubClient.Connection));
            }
            var searchResult = await _SearchClient.SearchUsers(new SearchUsersRequest(searchTerm));
            return searchResult.Items;
        }

        public async Task<IEnumerable<User>> GetCollaboratorsForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            if (_repoCollaboratorsClient == null)
            {
                _repoCollaboratorsClient = new RepoCollaboratorsClient(new ApiConnection(gitHubClient.Connection));
            }
            return await _repoCollaboratorsClient.GetAll(repositoryId);
        }

        public async Task<IEnumerable<User>> GetStargazersForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            if (_StarredClient == null)
            {
                _StarredClient = new StarredClient(new ApiConnection(gitHubClient.Connection));
            }
            return await _StarredClient.GetAllStargazers(repositoryId);
        }

        public async Task<IEnumerable<RepositoryContributor>> GetContributorsForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(gitHubClient.Connection));
            }
            return await _repositoriesClient.GetAllContributors(repositoryId);
        }

        public async Task<User> UpdateUser(UserUpdate updatedUserDetails, GitHubClient authorizedGitHubClient)
        {
            if (_usersClient == null)
            {
                _usersClient = new UsersClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _usersClient.Update(updatedUserDetails);
        }

        public async Task<bool> FollowUser(string userLogin, GitHubClient authorizedGitHubClient)
        {
            if (_usersClient == null)
            {
                _usersClient = new UsersClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _usersClient.Followers.Follow(userLogin);
        }

        public async Task UnfollowUser(string userLogin, GitHubClient authorizedGithubClient)
        {
            if (_usersClient == null)
            {
                _usersClient = new UsersClient(new ApiConnection(authorizedGithubClient.Connection));
            }
            await _usersClient.Followers.Unfollow(userLogin);
        }

        public async Task<IEnumerable<User>> GetFollowersForUser(string login, GitHubClient authorizedGithubClient)
        {
            if (_usersClient == null)
            {
                _usersClient = new UsersClient(new ApiConnection(authorizedGithubClient.Connection));
            }
            return await _usersClient.Followers.GetAll(login);
        }

        public async Task<IEnumerable<User>> GetFollowingForUser(string login, GitHubClient authorizedGithubClient)
        {
            if (_usersClient == null)
            {
                _usersClient = new UsersClient(new ApiConnection(authorizedGithubClient.Connection));
            }
            return await _usersClient.Followers.GetAllFollowing(login);
        }

    }
}