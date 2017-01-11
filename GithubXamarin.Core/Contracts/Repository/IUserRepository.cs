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
        Task<IEnumerable<User>> GetCollaboratorsOfRepository(long repositoryId, GitHubClient gitHubClient);
        Task<IEnumerable<User>> GetStargazersOfRepository(long repositoryId, GitHubClient gitHubClient);
        
    }
}