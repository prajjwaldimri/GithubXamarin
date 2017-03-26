using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Octokit;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IFileDataService
    {
        Task<Readme> GetReadme(long repositoryId, GitHubClient authorizeGitHubClient);
        Task<RepositoryContent> GetFile(long repositoryId, string filePath, GitHubClient authorizedGitHubClient);
        Task<ObservableCollection<RepositoryContent>> GetContentsOfDirectory(long repositoryId, string directoryPath, GitHubClient authorizedGitHubClient);
        Task<RepositoryContentChangeSet> CreateFile(long repositoryId, string filePath, string message, string content, GitHubClient authorizedGitHubClient);
        Task<RepositoryContentChangeSet> UpdateFile(long repositoryId, string filePath, string message, string content,
            string sha, GitHubClient authorizedGitHubClient);
        Task DeleteFile(long repositoryId, string filePath, string message, string sha, GitHubClient authorizedGitHubClient);
    }
}
