using System.Collections.Generic;
using Octokit;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IIssueDataService
    {
        Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesForCurrentUser(GitHubClient authorizedGithubClient);
        Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);
    }
}
