using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Gets a single issue in a repository.
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="issueNumber"></param>
        /// <param name="authorizedGitHubClient"></param>
        /// <returns></returns>
        public async Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            var issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            return await issuesClient.Get(repositoryId, issueNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="autohrizedGitHubClient"> GithubClient object that contains credentials.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Issue>> GetAllIssuesForCurrentUser(GitHubClient autohrizedGitHubClient)
        {
            return await autohrizedGitHubClient.Issue.GetAllForCurrent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="authorizedGitHubClient"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            var issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            return await issuesClient.GetAllForRepository(repositoryId);
        }

        /// <summary>
        /// Searches for issues in the whole git database.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="gitHubClient">Authorized Clients can also search private and org repos too.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            var searchClient = new SearchClient(new ApiConnection(gitHubClient.Connection));
            var searchResult = await searchClient.SearchIssues(new SearchIssuesRequest(searchTerm));
            return searchResult.Items;
        }

    }
}
