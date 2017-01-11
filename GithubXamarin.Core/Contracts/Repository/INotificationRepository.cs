using System.Collections.Generic;
using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface INotificationRepository
    {
        Task<Notification> GetNotificationById(int notificationId, GitHubClient gitHubClient);
        Task<IEnumerable<Notification>> GetAllNotificationsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Notification>> GetAllNotificationsForRepository(long repositoryId, GitHubClient gitHubClient);
        Task MarkNotificationAsRead(int notifiactionId, GitHubClient authorizedGitHubClient);
        Task MarkAllNotificationsAsRead(GitHubClient authorizedGitHubClient);
    }
}