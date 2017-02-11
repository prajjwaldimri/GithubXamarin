using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GithubXamarin.Core.Contracts.Repository
{
    public interface IFileRepository
    {
        Task<IEnumerable<RepositoryContent>> GetContentForRepository(long repositoryId, string path, GitHubClient gitHubClient);
        Task<IEnumerable<RepositoryContent>> GetRootContentForRepository(long repositoryId,
            GitHubClient githubClient);
        Task<Readme> GetReadmeForRepository(long repositoryId, GitHubClient authorizedGitHubClient);
        Task<RepositoryContentChangeSet> CreateFile(long repositoryId, string path, string message, string content,
            GitHubClient gitHubClient);
        Task<RepositoryContentChangeSet> UpdateFile(long repositoryId, string path, string message, string content, string sha, GitHubClient gitHubClient);
        Task DeleteFile(long repositoryId, string path, string message, string sha, GitHubClient gitHubClient);
    }
}
