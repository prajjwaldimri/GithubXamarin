using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Activity>> GetAllEventsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await _eventRepository.GetAllEventsForCurrentUser(authorizedGitHubClient);
        }

        public async Task<IEnumerable<Activity>> GetAllEventsOfRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return await _eventRepository.GetAllEventsOfRepository(repositoryId, gitHubClient);
        }

        public async Task<IEnumerable<Activity>> GetAllPublicEvents(GitHubClient gitHubClient)
        {
            return await _eventRepository.GetAllPublicEvents(gitHubClient);
        }

        public async Task<IEnumerable<Activity>> GetAllPublicEventsForUser(string userLogin, GitHubClient gitHubClient)
        {
            return await _eventRepository.GetAllPublicEventsForUser(userLogin, gitHubClient);
        }
    }
}
