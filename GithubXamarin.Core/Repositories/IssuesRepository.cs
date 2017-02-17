using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/issues/
    /// </summary>
    public class IssuesRepository : BaseRepository, IIssueRepository
    {
        private IssuesClient _issuesClient;

        /// <summary>
        /// Gets a single issue in a repository.
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="issueNumber"></param>
        /// <param name="authorizedGitHubClient"></param>
        /// <returns></returns>
        public async Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Get(repositoryId, issueNumber);
        }

        public async Task<Issue> GetIssueForRepository(string owner, string repoName, int issueNumber,
            GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Get(owner, repoName, issueNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorizedGitHubClient"> GithubClient object that contains credentials.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Issue>> GetAllIssuesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Issue.GetAllForCurrent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="authorizedGitHubClient"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.GetAllForRepository(repositoryId);
        }

        /// <summary>
        /// Searches for issues in the whole git database.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="gitHubClient">Authorized Clients can also search private and org repos too.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            if (_SearchClient == null)
            {
                _SearchClient = new SearchClient(new ApiConnection(gitHubClient.Connection));
            }
            var searchResult = await _SearchClient.SearchIssues(new SearchIssuesRequest(searchTerm));
            return searchResult.Items;
        }

    }
}
