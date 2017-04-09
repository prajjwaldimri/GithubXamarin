using System.Collections.ObjectModel;
using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IIssueDataService
    {
        Task<Issue> GetIssueForRepository(string owner, string repoName, int issueNumber,
            GitHubClient authorizedGitHubClient);

        Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient);

        Task<ObservableCollection<Issue>> GetAllOpenIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<ObservableCollection<Issue>> GetAllClosedIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<ObservableCollection<Issue>> GetAllOpenIssuesForCurrentUser(GitHubClient authorizedGithubClient);

        Task<ObservableCollection<Issue>> GetAllClosedIssuesForCurrentUser(GitHubClient authorizedGithubClient);

        Task<ObservableCollection<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);

        Task<Issue> CreateIssue(long repositoryId, NewIssue newIssueDetails, GitHubClient authorizedGithubClient);

        Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate updatedIssueDetails, GitHubClient authorizedGithubClient);

        Task UpdateLabels(long repositoryId, int issueNumber, string labels, GitHubClient authorizedGithubClient);

        Task<ObservableCollection<IssueComment>> GetCommentsForIssue(long repositoryId, int issueNumber,
            GitHubClient authorizedGithubClient);

        Task<IssueComment> GetComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);

        Task<IssueComment> CreateComment(long repositoryId, int issueNumber, string newComment, GitHubClient authorizedGitHubClient);

        Task<IssueComment> UpdateComment(long repositoryId, int issueNumber, string updatedComment, GitHubClient authorizedGitHubClient);

        Task<bool> DeleteComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);
    }
}
