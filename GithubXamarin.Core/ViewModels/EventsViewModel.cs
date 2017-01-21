using System.Collections.ObjectModel;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
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
            get { return _events;}
            set
            {
                _events = value;
                RaisePropertyChanged(()=>Events);
            }
        }

        #endregion

        public EventsViewModel(IGithubClientService githubClientService, IEventDataService eventDataService) : base(githubClientService)
        {
            _eventDataService = eventDataService;
        }

        public override void Start()
        {
            base.Start();
        }

        public async void Init(long? repositoryId=null, string userLogin=null)
        {
            if (repositoryId.HasValue)
            {
                Events = await _eventDataService.GetAllEventsOfRepository(repositoryId.Value,
                    _githubClientService.GetAuthorizedGithubClient());
            }
            else if (!string.IsNullOrWhiteSpace(userLogin))
            {
                Events = await _eventDataService.GetAllPublicEventsForUser(userLogin,
                    _githubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Events = await _eventDataService.GetAllEventsForCurrentUser(_githubClientService.GetAuthorizedGithubClient());
            }
        }
    }
}
