using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IIssueRepository
    {
        Task<Issue> GetIssueForRepository(string owner, string repoName, int issueNumber,
            GitHubClient authorizedGitHubClient);

        Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Issue>> GetAllIssuesForCurrentUser(GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);

        Task<Issue> CreateIssue(long repositoryId, NewIssue newIssueDetails, GitHubClient authorizedGitHubClient);

        Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate updatedIssueDetails, GitHubClient authorizedGitHubClient);

        Task UpdateLabels(long repositoryId, int issueNumber, string[] labels, GitHubClient authorizedGitHubClient);
    }
}
