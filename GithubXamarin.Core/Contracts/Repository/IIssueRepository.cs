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
        Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesOfRepository(long repositoryId, GitHubClient authorizedGitHubClient);
        Task<IEnumerable<Issue>> GetAllIssuesForCurrentUser(GitHubClient autohrizedGitHubClient);
        Task<Issue> GetIssueOfRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient);
    }
}
