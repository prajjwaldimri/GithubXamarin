using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    public class IssuesRepository : BaseRepository, IIssueRepository
    {
        public async Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient gitHubClient)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Issue>> GetIssuesForCurrentUser(GitHubClient gitHubClient)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Issue>> GetIssuesForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            throw new NotImplementedException();
        }
    }
}
