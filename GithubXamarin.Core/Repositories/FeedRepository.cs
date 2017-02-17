using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/activity/feeds/
    /// </summary>
    public class FeedRepository : BaseRepository, IFeedRepository
    {
        public async Task<Feed> GetFeedsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Activity.Feeds.GetFeeds();
        }
    }
}
