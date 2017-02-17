using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Octokit;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IGistDataService
    {
        Task<Gist> GetGist(string id, GitHubClient _authorizedGitHubClient);
        Task<ObservableCollection<Gist>> GetAllGistsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<ObservableCollection<Gist>> GetAllStarredGistsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<ObservableCollection<Gist>> GetAllPublicGistsForUser(string userName, GitHubClient githubClient);
    }
}
