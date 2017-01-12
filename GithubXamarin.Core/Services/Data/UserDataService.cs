using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<User>> GetCollaboratorsForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return await _userRepository.GetCollaboratorsForRepository(repositoryId, gitHubClient);
        }

        public async Task<User> GetCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await _userRepository.GetCurrentUser(authorizedGitHubClient);
        }

        public async Task<IEnumerable<User>> GetStargazersForRepository(long repositoryId, GitHubClient gitHubClient)
        {
            return await _userRepository.GetStargazersForRepository(repositoryId, gitHubClient);
        }

        public async Task<User> GetUser(string userLoginId, GitHubClient gitHubClient)
        {
            return await _userRepository.GetUser(userLoginId, gitHubClient);
        }

        public async Task<IEnumerable<User>> SearchUsers(string searchTerm, GitHubClient gitHubClient)
        {
            return await _userRepository.SearchUsers(searchTerm, gitHubClient);
        }
    }
}
