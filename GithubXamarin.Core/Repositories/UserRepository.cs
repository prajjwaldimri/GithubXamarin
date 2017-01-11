using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUser(string userLoginId, GitHubClient gitHubClient)
        {
            var usersClient = new UsersClient(new ApiConnection(gitHubClient.Connection));
            return await usersClient.Get(userLoginId);
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
            var searchClient = new SearchClient(new ApiConnection(gitHubClient.Connection));
            var searchResult = await searchClient.SearchUsers(new SearchUsersRequest(searchTerm));
            return searchResult.Items;
        }

        public async Task<IEnumerable<User>> GetCollaboratorsOfRepository(long repositoryId, GitHubClient gitHubClient)
        {
            var repoCollaboratorsClient = new RepoCollaboratorsClient(new ApiConnection(gitHubClient.Connection));
            return await repoCollaboratorsClient.GetAll(repositoryId);
        }

        public async Task<IEnumerable<User>> GetStargazersOfRepository(long repositoryId, GitHubClient gitHubClient)
        {
            var starredClient = new StarredClient(new ApiConnection(gitHubClient.Connection));
            return await starredClient.GetAllStargazers(repositoryId);
        }
        
    }
}