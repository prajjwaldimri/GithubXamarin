using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class FileDataService : IFileDataService
    {
        private readonly IFileRepository _fileRepository;

        public FileDataService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<RepositoryContent> GetFile(long repositoryId, string filePath, GitHubClient authorizedGitHubClient)
        {
            return (new ObservableCollection<RepositoryContent>(await _fileRepository.GetContentForRepository(repositoryId, filePath, authorizedGitHubClient)))[0];
        }

        public async Task<ObservableCollection<RepositoryContent>> GetContentsOfDirectory(long repositoryId, string directoryPath, GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<RepositoryContent>(await _fileRepository.GetContentForRepository(repositoryId, directoryPath, authorizedGitHubClient));
        }

        public async Task<RepositoryContentChangeSet> CreateFile(long repositoryId, string filePath, string message, string content, GitHubClient authorizedGitHubClient)
        {
            return await _fileRepository.CreateFile(repositoryId, filePath, message, content, authorizedGitHubClient);
        }

        public async Task<RepositoryContentChangeSet> UpdateFile(long repositoryId, string filePath, string message, string content, string sha, GitHubClient authorizedGitHubClient)
        {
            return await _fileRepository.UpdateFile(repositoryId, filePath, message, content, sha,
                authorizedGitHubClient);
        }

        public async Task DeleteFile(long repositoryId, string filePath, string message, string sha, GitHubClient authorizedGitHubClient)
        {
            await _fileRepository.DeleteFile(repositoryId, filePath, message, sha, authorizedGitHubClient);
        }

        public async Task<Readme> GetReadme(long repositoryId, GitHubClient authorizeGitHubClient)
        {
            return await _fileRepository.GetReadmeForRepository(repositoryId, authorizeGitHubClient);
        }
    }
}
