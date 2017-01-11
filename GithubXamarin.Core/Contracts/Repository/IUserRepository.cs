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
        Task<IEnumerable<User>> GetStargazersForRepository(long repositoryId, GitHubClient gitHubClient);
    }
}