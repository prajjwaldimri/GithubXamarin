using System.Collections.ObjectModel;
using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IUserDataService
    {
        Task<User> GetUser(string userLogin, GitHubClient gitHubClient);
        Task<User> GetCurrentUser(GitHubClient authorizedGitHubClient);
        Task<ObservableCollection<User>> SearchUsers(string searchTerm, GitHubClient gitHubClient);
        Task<ObservableCollection<User>> GetCollaboratorsForRepository(long repositoryId, GitHubClient gitHubClient);
        Task<ObservableCollection<RepositoryContributor>> GetContributorsForRepository(long repositoryId, GitHubClient gitHubClient);
        Task<ObservableCollection<User>> GetStargazersForRepository(long repositoryId, GitHubClient gitHubClient);
    }
}
