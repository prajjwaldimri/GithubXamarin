using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IIssueRepository
    {
        Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesForCurrentUser(GitHubClient autohrizedGitHubClient);
        Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);
    }
}
