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

        public async Task<IEnumerable<Issue>> GetAllOpenIssuesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Issue.GetAllForCurrent();
        }

        public async Task<IEnumerable<Issue>> GetAllClosedIssuesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Issue.GetAllForCurrent(new IssueRequest { State = ItemStateFilter.Closed });
        }

        public async Task<IEnumerable<Issue>> GetAllOpenIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.GetAllForRepository(repositoryId);
        }

        public async Task<IEnumerable<Issue>> GetAllClosedIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.GetAllForRepository(repositoryId, new RepositoryIssueRequest{State = ItemStateFilter.Closed});
        }

        public async Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            if (_SearchClient == null)
            {
                _SearchClient = new SearchClient(new ApiConnection(gitHubClient.Connection));
            }
            var searchResult = await _SearchClient.SearchIssues(new SearchIssuesRequest(searchTerm));
            return searchResult.Items;
        }

        public async Task<Issue> CreateIssue(long repositoryId, NewIssue newIssueDetails, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Create(repositoryId, newIssueDetails);
        }

        public async Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate updatedIssueDetails,
            GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Update(repositoryId, issueNumber, updatedIssueDetails);
        }

        public async Task UpdateLabels(long repositoryId, int issueNumber, string[] labels, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            await _issuesClient.Labels.AddToIssue(repositoryId, issueNumber, labels);
        }
    }
}
