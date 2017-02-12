using System.Collections.ObjectModel;
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

        public async Task<ObservableCollection<Issue>> GetAllIssuesForCurrentUser(GitHubClient authorizedGithubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.GetAllIssuesForCurrentUser(authorizedGithubClient));
        }

        public async Task<ObservableCollection<Issue>> GetAllIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.GetAllIssuesForRepository(repositoryId, authorizedGitHubClient));
        }

        public async Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.GetIssueForRepository(repositoryId, issueNumber, authorizedGitHubClient);
        }

        public async Task<ObservableCollection<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.SearchIssues(searchTerm, gitHubClient));
        }

        public async Task<Issue> GetIssueForRepository(string owner, string repoName, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.GetIssueForRepository(owner, repoName, issueNumber, authorizedGitHubClient);
        }
    }
}
