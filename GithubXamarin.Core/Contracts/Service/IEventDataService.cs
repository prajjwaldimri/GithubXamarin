using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Octokit;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IEventDataService
    {
        Task<ObservableCollection<Activity>> GetAllPublicEvents(GitHubClient gitHubClient);
        Task<ObservableCollection<Activity>> GetAllEventsOfRepository(long repositoryId, GitHubClient gitHubClient);
        Task<ObservableCollection<Activity>> GetAllEventsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<ObservableCollection<Activity>> GetAllPublicEventsForUser(string userLogin, GitHubClient gitHubClient);
    }
}
