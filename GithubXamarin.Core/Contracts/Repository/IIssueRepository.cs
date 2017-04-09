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

        Task<IEnumerable<Issue>> GetAllOpenIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Issue>> GetAllClosedIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Issue>> GetAllOpenIssuesForCurrentUser(GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Issue>> GetAllClosedIssuesForCurrentUser(GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);

        Task<Issue> CreateIssue(long repositoryId, NewIssue newIssueDetails, GitHubClient authorizedGitHubClient);

        Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate updatedIssueDetails, GitHubClient authorizedGitHubClient);

        Task UpdateLabels(long repositoryId, int issueNumber, string[] labels, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<IssueComment>> GetCommentsForIssue(long repositoryId, int issueNumber,
            GitHubClient authorizedGithubClient);

        Task<IssueComment> GetComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);

        Task<IssueComment> CreateComment(long repositoryId, int issueNumber, string newComment, GitHubClient authorizedGitHubClient);

        Task<IssueComment> UpdateComment(long repositoryId, int issueNumber, string updatedComment, GitHubClient authorizedGitHubClient);

        Task DeleteComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);
    }
}
