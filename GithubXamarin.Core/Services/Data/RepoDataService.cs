using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class RepoDataService : IRepoDataService
    {
        private readonly IRepoRepository _repoRepository;

        public RepoDataService(IRepoRepository repoRepository)
        {
            _repoRepository = repoRepository;
        }

        public async Task<Repository> ForkRepository(long repositoryId, GitHubClient authorizedGithubClient)
        {
            return await _repoRepository.ForkRepository(repositoryId, authorizedGithubClient);
        }

        public async Task<ObservableCollection<Repository>> GetAllRepositoriesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Repository>(await _repoRepository.GetAllRepositoriesForCurrentUser(authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Repository>> GetAllRepositoriesForUser(string login, GitHubClient gitHubClient)
        {
            return new ObservableCollection<Repository>(await _repoRepository.GetAllRepositoriesForUser(login, gitHubClient));
        }

        public async Task<Repository> GetRepository(long repositoryId, GitHubClient githubClient)
        {
            return await _repoRepository.GetRepository(repositoryId, githubClient);
        }

        public async Task<ObservableCollection<Repository>> SearchRepositories(string searchTerm, GitHubClient githubClient)
        {
            return new ObservableCollection<Repository>(await _repoRepository.SearchRepositories(searchTerm, githubClient));
        }

        public async Task<bool> StarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGithubClient)
        {
            return await _repoRepository.StarRepository(repositoryOwner, repositoryName, authorizedGithubClient);
        }

        public async Task<bool> UnStarRepository(string repositoryOwner, string repositoryName, GitHubClient authorizedGithubClient)
        {
            return await _repoRepository.UnStarRepository(repositoryOwner, repositoryName, authorizedGithubClient);
        }
    }
}
