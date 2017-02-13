using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
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

        private int _selectedEvent;
        public int SelectedEvent
        {
            get { return _selectedEvent;}
            set
            {
                _selectedEvent = value;
                RaisePropertyChanged(() => SelectedEvent);
            }
        }

        private ICommand _eventClickCommand;
        public ICommand EventClickCommand
        {
            get
            {
                _eventClickCommand = _eventClickCommand ?? new MvxCommand(NavigateToEventType);
                return _eventClickCommand;
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

            try
            {
                if (repositoryId.HasValue)
                {
                    Events = await _eventDataService.GetAllEventsOfRepository(repositoryId.Value,
                        GithubClientService.GetAuthorizedGithubClient());
                    Messenger.Publish(new AppBarHeaderChangeMessage(this)
                    {
                        HeaderTitle = $"Events for {Events[0]?.Repo.FullName}"
                    });
                }
                else if (!string.IsNullOrWhiteSpace(userLogin))
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this)
                    {
                        HeaderTitle = $"Public Events for {userLogin}"
                    });
                    Events = await _eventDataService.GetAllPublicEventsForUser(userLogin,
                        GithubClientService.GetAuthorizedGithubClient());
                }
                else
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) {HeaderTitle = "Your Events"});
                    Events =
                        await _eventDataService.GetAllEventsForCurrentUser(
                            GithubClientService.GetAuthorizedGithubClient());
                }
            }
            catch (HttpRequestException)
            {
                await DialogService.ShowDialogASync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private void NavigateToEventType()
        {
            var activity = Events?[SelectedEvent];
            if (activity == null) return;
            switch (activity.Type)
            {
                case "CommitCommentEvent":
                    break;
                case "CreateEvent":
                    ShowViewModel<RepositoryViewModel>(new {repositoryId = activity.Repo.Id});
                    break;
                case "DeleteEvent":
                    break;
                case "ForkEvent":
                    var forkEventPayload = activity.Payload as ForkEventPayload;
                    ShowViewModel<RepositoryViewModel>(new { repositoryId = forkEventPayload.Forkee.Id });
                    break;
                case "GollumEvent":
                    break;
                case "IssuesEvent":
                    var issueEventPayload = activity.Payload as IssueEventPayload;
                    ShowViewModel<IssueViewModel>(new {issueNumber = issueEventPayload.Issue.Number, repositoryId = issueEventPayload.Repository.Id});
                    break;
                case "IssueCommentEvent":
                    break;
                case "LabelEvent":
                    break;
                case "MemberEvent":
                    break;
                case "ProjectCardEvent":
                    break;
                case "ProjectEvent":
                    break;
                case "PublicEvent":
                    break;
                case "PullRequestEvent":
                    break;
                case "PushEvent":
                    ShowViewModel<RepositoryViewModel>(new { repositoryId = activity.Repo.Id });
                    break;
                case "ReleaseEvent":
                    break;
                case "WatchEvent":
                    ShowViewModel<RepositoryViewModel>(new { repositoryId = activity.Repo.Id });
                    break;
                default:
                    break;
            }
        }
    }
}
