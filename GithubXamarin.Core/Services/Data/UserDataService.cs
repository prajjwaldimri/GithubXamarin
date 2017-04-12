using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserRepository _userRepository;
        public UserDataService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ObservableCollection<User>> GetCollaboratorsForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return new ObservableCollection<User>(await _userRepository.GetCollaboratorsForRepository(repositoryId, gitHubClient));
        }

        public async Task<ObservableCollection<RepositoryContributor>> GetContributorsForRepository(long repositoryId,
            GitHubClient gitHubClient)
        {
            return new ObservableCollection<RepositoryContributor>(await _userRepository.GetContributorsForRepository(repositoryId, gitHubClient));
        }

        public async Task<User> GetCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await _userRepository.GetCurrentUser(authorizedGitHubClient);
        }

        public async Task<ObservableCollection<User>> GetStargazersForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return new ObservableCollection<User>(await _userRepository.GetStargazersForRepository(repositoryId, gitHubClient));
        }

        public async Task<User> GetUser(string userLogin, GitHubClient gitHubClient)
        {
            return await _userRepository.GetUser(userLogin, gitHubClient);
        }

        public async Task<ObservableCollection<User>> SearchUsers(string searchTerm, GitHubClient gitHubClient)
        {
            return new ObservableCollection<User>(await _userRepository.SearchUsers(searchTerm, gitHubClient));
        }

        public async Task<User> UpdateUser(UserUpdate updatedUserDetails, GitHubClient authorizedGitHubClient)
        {
            return await _userRepository.UpdateUser(updatedUserDetails, authorizedGitHubClient);
        }

        public async Task<bool> FollowUser(string userLogin, GitHubClient authorizedGitHubClient)
        {
            return await _userRepository.FollowUser(userLogin, authorizedGitHubClient);
        }

        public async Task UnfollowUser(string userLogin, GitHubClient authorizedGitHubClient)
        {
            await _userRepository.UnfollowUser(userLogin, authorizedGitHubClient);
        }

        public async Task<ObservableCollection<User>> GetFollowersForUser(string login, GitHubClient authorizedGithubClient)
        {
            return new ObservableCollection<User>(await _userRepository.GetFollowersForUser(login, authorizedGithubClient));
        }

        public async Task<ObservableCollection<User>> GetFollowingForUser(string login, GitHubClient authorizedGithubClient)
        {
            return new ObservableCollection<User>(await _userRepository.GetFollowingForUser(login, authorizedGithubClient));
        }
    }
}
