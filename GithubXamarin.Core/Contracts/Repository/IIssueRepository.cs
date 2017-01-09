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
        Task<IEnumerable<Issue>> GetIssuesForRepository(long repositoryId, GitHubClient gitHubClient);
        Task<IEnumerable<Issue>> GetIssuesForCurrentUser(GitHubClient gitHubClient);
        Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient gitHubClient);
    }
}
