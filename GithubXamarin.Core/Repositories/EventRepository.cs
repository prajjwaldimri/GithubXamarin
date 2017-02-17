using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/activity/events/
    /// </summary>
    public class EventRepository : BaseRepository, IEventRepository
    {
        public async Task<IEnumerable<Activity>> GetAllEventsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            var currentUser = await authorizedGitHubClient.User.Current();
            return await authorizedGitHubClient.Activity.Events.GetAllUserReceived(currentUser.Login);
        }

        public async Task<IEnumerable<Activity>> GetAllEventsOfRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return await gitHubClient.Activity.Events.GetAllForRepository(repositoryId);
        }

        public async Task<IEnumerable<Activity>> GetAllPublicEvents(GitHubClient gitHubClient)
        {
            return await gitHubClient.Activity.Events.GetAll();
        }

        public async Task<IEnumerable<Activity>> GetAllPublicEventsForUser(string userLogin, GitHubClient gitHubClient)
        {
            return await gitHubClient.Activity.Events.GetAllUserPerformedPublic(userLogin);
        }
    }
}