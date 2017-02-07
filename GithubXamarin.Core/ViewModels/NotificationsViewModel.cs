using System.Collections.ObjectModel;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Plugins.Network.Reachability;
using MvvmCross.Plugins.Network.Rest;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class NotificationsViewModel : BaseViewModel, INotificationsViewModel
    {
        #region Properties and Commands

        private readonly INotificationDataService _notificationDataService;

        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications;}
            set
            {
                _notifications = value;
                RaisePropertyChanged(() => Notifications);
            }
        }

        #endregion


        public NotificationsViewModel(IGithubClientService githubClientService, INotificationDataService notificationDataService) : base(githubClientService)
        {
            _notificationDataService = notificationDataService;
        }

        public async void Init(long? repositoryId=null)
        {
            if (IsInternetAvailable())
            {
                if (repositoryId.HasValue)
                {
                    Notifications = await _notificationDataService.GetAllNotificationsForRepository(repositoryId.Value,
                        GithubClientService.GetAuthorizedGithubClient());
                }
                else
                {
                    Notifications =
                        await _notificationDataService.GetAllNotificationsForCurrentUser(
                            GithubClientService.GetAuthorizedGithubClient());
                }
            }
        }
    }
}
