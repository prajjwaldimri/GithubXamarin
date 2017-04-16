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

        Task<IEnumerable<Label>> GetLabelsForRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task AddLabelsToIssue(long repositoryId, int issueNumber, string[] labels, GitHubClient authorizedGitHubClient);

        Task ReplaceLabelsForIssue(long repositoryId, int issueNumber, string[] labels,
            GitHubClient authorizedGitHubClient);

        Task<IEnumerable<IssueComment>> GetCommentsForIssue(long repositoryId, int issueNumber,
            GitHubClient authorizedGithubClient);

        Task<IssueComment> GetComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);

        Task<IssueComment> CreateComment(long repositoryId, int issueNumber, string newComment, GitHubClient authorizedGitHubClient);

        Task<IssueComment> UpdateComment(long repositoryId, int issueNumber, string updatedComment, GitHubClient authorizedGitHubClient);

        Task DeleteComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<Milestone>> GetMilestonesForRepository(long repoId, GitHubClient authorizedGitHubClient);

        Task<Milestone> GetMilestone(long repoId, int number, GitHubClient authorizedGitHubClient);

        Task<Milestone> CreateMilestone(long repoId, NewMilestone newMilestone, GitHubClient authorizedGitHubClient);

        Task<Milestone> UpdateMilestone(long repoId, int number, MilestoneUpdate milestoneUpdate,
            GitHubClient authorizedGitHubClient);

        Task DeleteMilestone(long repoId, int number, GitHubClient authorizedGitHubClient);

        Task<IEnumerable<User>> GetAllPossibleAssignees(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<Issue> AddAssigneesToIssue(string owner, string name, int number, AssigneesUpdate assigneesUpdate,
            GitHubClient authorizedGitHubClient);

        Task<Issue> RemoveAssigneesFromIssue(string owner, string name, int number, AssigneesUpdate assigneesUpdate,
            GitHubClient authorizedGitHubClient);
    }
}
