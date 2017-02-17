using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IGistRepository
    {
        Task<Gist> GetGist(string id, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Gist>> GetAllGistsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Gist>> GetAllStarredGistsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Gist>> GetAllPublicGistsForUser(string userName, GitHubClient githubClient);
    }
}
