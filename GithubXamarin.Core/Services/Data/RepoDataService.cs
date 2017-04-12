using System;
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
            return new ObservableCollection<Repository>(await _repoRepository.GetAllRepositoriesForCurrentUser
                (authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Repository>> GetAllStarredRepositoriesForCurrentUser(GitHubClient
            authorizedGitHubClient)
        {
            return new ObservableCollection<Repository>(await _repoRepository.GetAllStarredRepositoriesForCurrentUser
                (authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Repository>> GetAllRepositoriesForUser(string login,
            GitHubClient gitHubClient)
        {
            return new ObservableCollection<Repository>(await _repoRepository.GetAllStarredRepositoriesForUser(login,
                gitHubClient));
        }

        public async Task<ObservableCollection<Repository>> GetAllStarredRepositoriesForUser(string login,
            GitHubClient gitHubClient)
        {
            return new ObservableCollection<Repository>(await _repoRepository.GetAllRepositoriesForUser(login, gitHubClient));
        }

        public async Task<Repository> GetRepository(long repositoryId, GitHubClient githubClient)
        {
            return await _repoRepository.GetRepository(repositoryId, githubClient);
        }

        public async Task<ObservableCollection<RepositoryContent>> GetContentsOfRepository(long repoId,
            GitHubClient authorizedGitHubClient, string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return new ObservableCollection<RepositoryContent>(await _repoRepository.GetContentsOfRepository(repoId,
                    authorizedGitHubClient));
            }
            return new ObservableCollection<RepositoryContent>(await _repoRepository.GetContentsOfRepository(repoId,
                authorizedGitHubClient, path));
        }

        public async Task<ObservableCollection<Repository>> SearchRepositories(string searchTerm, GitHubClient githubClient)
        {
            return new ObservableCollection<Repository>(await _repoRepository.SearchRepositories(searchTerm, githubClient));
        }

        public async Task<bool> StarRepository(string repositoryOwner, string repositoryName,
            GitHubClient authorizedGithubClient)
        {
            return await _repoRepository.StarRepository(repositoryOwner, repositoryName, authorizedGithubClient);
        }

        public async Task<bool> UnStarRepository(string repositoryOwner, string repositoryName,
            GitHubClient authorizedGithubClient)
        {
            return await _repoRepository.UnStarRepository(repositoryOwner, repositoryName, authorizedGithubClient);
        }

        public async Task<Subscription> WatchRepository(long repositoryId, NewSubscription subscription, GitHubClient authorizedGitHubClient)
        {
            return await _repoRepository.WatchRepository(repositoryId, subscription, authorizedGitHubClient);
        }

        public async Task<bool> UnWatchRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            return await _repoRepository.UnWatchRepository(repositoryId, authorizedGitHubClient);
        }

        public async Task<Repository> CreateRepository(NewRepository newRepositoryDetails, GitHubClient authorizedGitHubClient)
        {
            return await _repoRepository.CreateRepository(newRepositoryDetails, authorizedGitHubClient);
        }

        public async Task<Repository> UpdateRepository(long repositoryId, RepositoryUpdate updatedRepositoryDetails,
            GitHubClient authorizedGitHubClient)
        {
            return await _repoRepository.UpdateRepository(repositoryId, updatedRepositoryDetails, authorizedGitHubClient);
        }

        public async Task<bool> DeleteRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            await _repoRepository.DeleteRepository(repositoryId, authorizedGitHubClient);
            try
            {
                var repo = await _repoRepository.GetRepository(repositoryId, authorizedGitHubClient);
                if (repo != null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
            return true;
        }
    }
}
