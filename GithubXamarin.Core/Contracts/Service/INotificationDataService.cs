using System.Collections.ObjectModel;
using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface INotificationDataService
    {
        Task<Notification> GetNotificationById(int notificationId, GitHubClient gitHubClient);
        Task<ObservableCollection<Notification>> GetAllNotificationsForCurrentUser(GitHubClient authorizedGitHubClient);
        Task<ObservableCollection<Notification>> GetAllNotificationsForRepository(long repositoryId, GitHubClient gitHubClient);
        Task MarkNotificationAsRead(int notificationId, GitHubClient authorizedGitHubClient);
        Task MarkAllNotificationsAsRead(GitHubClient authorizedGitHubClient);
    }
}
