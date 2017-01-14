using System.Threading.Tasks;
using Octokit;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IFeedDataService
    {
        Task<Feed> GetFeedsForCurrentUser(GitHubClient authorizedGitHubClient);
    }
}
