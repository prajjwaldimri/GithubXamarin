using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class FeedDataService : IFeedDataService
    {
        private readonly IFeedRepository _feedRepository;

        public FeedDataService(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }

        public async Task<Feed> GetFeedsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await _feedRepository.GetFeedsForCurrentUser(authorizedGitHubClient);
        }
    }
}
