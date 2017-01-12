using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class IssueDataService : IIssueDataService
    {
        private readonly IIssueRepository _issueRepository;

        public IssueDataService(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<IEnumerable<Issue>> GetAllIssuesForCurrentUser(GitHubClient authorizedGithubClient)
        {
            return await _issueRepository.GetAllIssuesForCurrentUser(authorizedGithubClient);
        }

        public async Task<IEnumerable<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.GetAllIssuesForRepository(repositoryId, authorizedGitHubClient);
        }

        public async Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.GetIssueForRepository(repositoryId, issueNumber, authorizedGitHubClient);
        }

        public async Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            return await _issueRepository.SearchIssues(searchTerm, gitHubClient);
        }
    }
}
