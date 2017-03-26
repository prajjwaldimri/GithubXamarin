using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// Handles all tasks related to files on Github servers using Octokit
    /// https://developer.github.com/v3/repos/contents/
    /// </summary>
    public class FileRepository : BaseRepository, IFileRepository
    {
        private RepositoriesClient _repositoriesClient;
        /// <summary>
        /// Returns the content of a file or a directory path inside repository.
        /// https://developer.github.com/v3/repos/contents/#get-contents
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="path"></param>
        /// <param name="gitHubClient"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RepositoryContent>> GetContentForRepository(long repositoryId, string path, GitHubClient gitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(gitHubClient.Connection));
            }
            return await _repositoriesClient.Content.GetAllContents(repositoryId, path);
        }

        /// <summary>
        /// Returns the contents from root of repo.
        /// https://developer.github.com/v3/repos/contents/#get-contents
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="githubClient"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RepositoryContent>> GetRootContentForRepository(long repositoryId,
            GitHubClient githubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(githubClient.Connection));
            }
            return await _repositoriesClient.Content.GetAllContents(repositoryId);
        }

        public async Task<Readme> GetReadmeForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _repositoriesClient.Content.GetReadme(repositoryId);
        }

        public async Task<RepositoryContentChangeSet> CreateFile(long repositoryId, string path, string message, string content, GitHubClient gitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(gitHubClient.Connection));
            }
            return await _repositoriesClient.Content.CreateFile(repositoryId, path, new CreateFileRequest(message, content));
        }

        public async Task<RepositoryContentChangeSet> UpdateFile(long repositoryId, string path, string message, string content, string sha,
            GitHubClient gitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(gitHubClient.Connection));
            }
            return await _repositoriesClient.Content.UpdateFile(repositoryId, path, new UpdateFileRequest(message, content, sha));
        }

        public async Task DeleteFile(long repositoryId, string path, string message, string sha, GitHubClient gitHubClient)
        {
            if (_repositoriesClient == null)
            {
                _repositoriesClient = new RepositoriesClient(new ApiConnection(gitHubClient.Connection));
            }
            await _repositoriesClient.Content.DeleteFile(repositoryId, path, new DeleteFileRequest(message, sha));
        }
    }
}
