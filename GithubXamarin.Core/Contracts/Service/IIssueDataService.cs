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
    }
}
