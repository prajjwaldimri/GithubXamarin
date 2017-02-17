using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/gists/#gists
    /// </summary>
    public class GistRepository : BaseRepository, IGistRepository
    {
        private GistsClient _gistsClient;

        public async Task<Gist> GetGist(string id, GitHubClient authorizedGitHubClient)
        {
            if (_gistsClient == null)
            {
                _gistsClient = new GistsClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _gistsClient.Get(id);
        }

        public async Task<IEnumerable<Gist>> GetAllGistsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            if (_gistsClient == null)
            {
                _gistsClient = new GistsClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _gistsClient.GetAll();
        }

        public async Task<IEnumerable<Gist>> GetAllStarredGistsForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            if (_gistsClient == null)
            {
                _gistsClient = new GistsClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _gistsClient.GetAllStarred();
        }

        public async Task<IEnumerable<Gist>> GetAllPublicGistsForUser(string userName, GitHubClient githubClient)
        {
            if (_gistsClient == null)
            {
                _gistsClient = new GistsClient(new ApiConnection(githubClient.Connection));
            }
            return await _gistsClient.GetAllForUser(userName);
        }
    }
}
