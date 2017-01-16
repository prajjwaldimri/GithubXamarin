using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class EventDataService : IEventDataService
    {
        private readonly IEventRepository _eventRepository;

        public EventDataService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<ObservableCollection<Activity>> GetAllEventsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Activity>(await _eventRepository.GetAllEventsForCurrentUser(authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Activity>> GetAllEventsOfRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return new ObservableCollection<Activity>(await _eventRepository.GetAllEventsOfRepository(repositoryId, gitHubClient));
        }

        public async Task<ObservableCollection<Activity>> GetAllPublicEvents(GitHubClient gitHubClient)
        {
            return new ObservableCollection<Activity>(await _eventRepository.GetAllPublicEvents(gitHubClient));
        }

        public async Task<ObservableCollection<Activity>> GetAllPublicEventsForUser(string userLogin, GitHubClient gitHubClient)
        {
            return new ObservableCollection<Activity>(await _eventRepository.GetAllPublicEventsForUser(userLogin, gitHubClient));
        }
    }
}
