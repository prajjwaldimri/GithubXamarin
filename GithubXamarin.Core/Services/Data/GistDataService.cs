using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    /// <summary>
    /// https://developer.github.com/v3/gists/#gists
    /// </summary>
    public class GistDataService : IGistDataService
    {
        private readonly IGistRepository _gistRepository;

        public GistDataService(IGistRepository gistRepository)
        {
            _gistRepository = gistRepository;
        }
        
        public async Task<Gist> GetGist(string id, GitHubClient _authorizedGitHubClient)
        {
            return await _gistRepository.GetGist(id, _authorizedGitHubClient);
        }

        public async Task<ObservableCollection<Gist>> GetAllGistsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Gist>(await _gistRepository.GetAllGistsForCurrentUser(authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Gist>> GetAllStarredGistsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Gist>(await _gistRepository.GetAllStarredGistsForCurrentUser(authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Gist>> GetAllPublicGistsForUser(string userName, GitHubClient githubClient)
        {
            return new ObservableCollection<Gist>(await _gistRepository.GetAllPublicGistsForUser(userName, githubClient));
        }
    }
}
