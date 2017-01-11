using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IEventRepository
    {
        Task<IEnumerable<Activity>> GetAllPublicEvents(GitHubClient gitHubClient);
        Task<IEnumerable<Activity>> GetAllEventsOfRepository(long repositoryId, GitHubClient gitHubClient);
        Task<IEnumerable<Activity>> GetAllEventsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Activity>> GetAllPublicEventsForUser(string userLogin, GitHubClient gitHubClient);
    }
}