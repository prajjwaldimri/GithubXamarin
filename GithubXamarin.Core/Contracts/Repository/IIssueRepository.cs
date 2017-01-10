using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IIssueRepository
    {
        Task<Issue> GetIssueOfRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesOfRepository(long repositoryId, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesOfCurrentUser(GitHubClient autohrizedGitHubClient);
        Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);
    }
}
