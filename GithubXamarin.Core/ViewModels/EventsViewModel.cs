using System.Collections.ObjectModel;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class EventsViewModel : BaseViewModel, IEventsViewModel
    {
        #region Properties and Commands

        private readonly IEventDataService _eventDataService;

        private ObservableCollection<Activity> _events;
        public ObservableCollection<Activity> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }

        #endregion

        public EventsViewModel(IGithubClientService githubClientService, IEventDataService eventDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _eventDataService = eventDataService;
        }

        public async void Init(long? repositoryId = null, string userLogin = null)
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("Use this moment to look up from your screen and enjoy life.", "No Internet Connection!");
                return;
            }

            Messenger.Publish(new LoadingStatusMessage(this) {IsLoadingIndicatorActive = true});

            if (repositoryId.HasValue)
            {
                Events = await _eventDataService.GetAllEventsOfRepository(repositoryId.Value,
                    GithubClientService.GetAuthorizedGithubClient());
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Events for {Events[0]?.Repo.FullName}" });
            }
            else if (!string.IsNullOrWhiteSpace(userLogin))
            {
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Public Events for {userLogin}" });
                Events = await _eventDataService.GetAllPublicEventsForUser(userLogin,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Your Events" });
                Events =
                    await _eventDataService.GetAllEventsForCurrentUser(
                        GithubClientService.GetAuthorizedGithubClient());
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
