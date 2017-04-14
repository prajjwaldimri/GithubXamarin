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

        Task<ObservableCollection<Label>> GetLabelsForRepository(long repositoryId,
            GitHubClient authorizedGitHubClient);

        Task AddLabelsToIssue(long repositoryId, int issueNumber, string labels, GitHubClient authorizedGithubClient);

        Task ReplaceLabelsForIssue(long repositoryId, int issueNumber, string labels,
            GitHubClient authorizedGitHubClient);

        Task<ObservableCollection<IssueComment>> GetCommentsForIssue(long repositoryId, int issueNumber,
            GitHubClient authorizedGithubClient);

        Task<IssueComment> GetComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);

        Task<IssueComment> CreateComment(long repositoryId, int issueNumber, string newComment, GitHubClient authorizedGitHubClient);

        Task<IssueComment> UpdateComment(long repositoryId, int issueNumber, string updatedComment, GitHubClient authorizedGitHubClient);

        Task<bool> DeleteComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);

        Task<ObservableCollection<Milestone>> GetMilestonesForRepository(long repoId, GitHubClient authorizedGitHubClient);

        Task<Milestone> CreateMilestone(long repoId, NewMilestone newMilestone, GitHubClient authorizedGitHubClient);

        Task<Milestone> UpdateMilestone(long repoId, int number, MilestoneUpdate milestoneUpdate,
            GitHubClient authorizedGitHubClient);

        Task<bool> DeleteMilestone(long repoId, int number, GitHubClient authorizedGitHubClient);

        Task<ObservableCollection<User>> GetAllPossibleAssignees(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<Issue> AddAssigneesToIssue(string owner, string name, int number, string assignees,
            GitHubClient authorizedGitHubClient);

        Task<Issue> RemoveAssigneesFromIssue(string owner, string name, int number, string assignees,
            GitHubClient authorizedGitHubClient);
    }
}
