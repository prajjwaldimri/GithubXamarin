using System.Collections.Generic;
using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userLoginId, GitHubClient gitHubClient);

        Task<User> GetCurrentUser(GitHubClient authorizedGitHubClient);

        Task<IEnumerable<User>> SearchUsers(string searchTerm, GitHubClient gitHubClient);

        Task<IEnumerable<User>> GetCollaboratorsForRepository(long repositoryId, GitHubClient gitHubClient);

        Task<IEnumerable<RepositoryContributor>> GetContributorsForRepository(long repositoryId, GitHubClient gitHubClient);

        Task<IEnumerable<User>> GetStargazersForRepository(long repositoryId, GitHubClient gitHubClient);

        Task<User> UpdateUser(UserUpdate updatedUserDetails, GitHubClient authorizedGitHubClient);

        Task<bool> FollowUser(string userLogin, GitHubClient authorizedGitHubClient);

        Task UnfollowUser(string userLogin, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<User>> GetFollowersForUser(string login, GitHubClient authorizedGithubClient);

        Task<IEnumerable<User>> GetFollowingForUser(string login, GitHubClient authorizedGithubClient);
    }
}