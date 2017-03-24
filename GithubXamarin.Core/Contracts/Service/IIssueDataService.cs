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

        Task<ObservableCollection<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);

        Task<ObservableCollection<Issue>> GetAllIssuesForCurrentUser(GitHubClient authorizedGithubClient);

        Task<ObservableCollection<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);

        Task<Issue> CreateIssue(long repositoryId, NewIssue newIssueDetails, GitHubClient authorizedGithubClient);

        Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate updatedIssueDetails, GitHubClient authorizedGithubClient);

        Task UpdateLabels(long repositoryId, int issueNumber, string labels, GitHubClient authorizedGithubClient);
    }
}
