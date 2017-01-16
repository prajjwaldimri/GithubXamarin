using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class NotificationDataService : INotificationDataService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationDataService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<ObservableCollection<Notification>> GetAllNotificationsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Notification>(await _notificationRepository.GetAllNotificationsForCurrentUser(authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Notification>> GetAllNotificationsForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return new ObservableCollection<Notification>(await _notificationRepository.GetAllNotificationsForRepository(repositoryId, gitHubClient));
        }

        public async Task<Notification> GetNotificationById(int notificationId, GitHubClient gitHubClient)
        {
            return await _notificationRepository.GetNotificationById(notificationId, gitHubClient);
        }

        public async Task MarkAllNotificationsAsRead(GitHubClient authorizedGitHubClient)
        {
            await _notificationRepository.MarkAllNotificationsAsRead(authorizedGitHubClient);
        }

        public async Task MarkNotificationAsRead(int notificationId, GitHubClient authorizedGitHubClient)
        {
            await _notificationRepository.MarkNotificationAsRead(notificationId, authorizedGitHubClient);
        }
    }
}
