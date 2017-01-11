using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/activity/feeds/
    /// </summary>
    public class FeedRepository : IFeedRepository
    {
        public async Task<Feed> GetFeedsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Activity.Feeds.GetFeeds();
        }
    }
}
