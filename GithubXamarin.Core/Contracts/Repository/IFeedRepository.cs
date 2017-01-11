using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IFeedRepository
    {
        Task<Feed> GetFeedsForCurrentUser(GitHubClient authorizedGitHubClient);
    }
}