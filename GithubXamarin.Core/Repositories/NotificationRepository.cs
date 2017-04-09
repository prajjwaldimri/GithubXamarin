using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/activity/notifications/
    /// </summary>
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public async Task<IEnumerable<Notification>> GetAllNotificationsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Activity.Notifications.GetAllForCurrent(new NotificationsRequest() { Participating = true });
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return await gitHubClient.Activity.Notifications.GetAllForRepository(repositoryId);
        }

        public async Task<Notification> GetNotificationById(int notificationId, GitHubClient gitHubClient)
        {
            return await gitHubClient.Activity.Notifications.Get(notificationId);
        }

        public async Task MarkAllNotificationsAsRead(GitHubClient authorizedGitHubClient)
        {
            await authorizedGitHubClient.Activity.Notifications.MarkAsRead();
        }

        public async Task MarkNotificationAsRead(int notificationId, GitHubClient authorizedGitHubClient)
        {
            await authorizedGitHubClient.Activity.Notifications.MarkAsRead(notificationId);
        }
    }
}