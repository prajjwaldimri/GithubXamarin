using Octokit;

namespace GithubXamarin.Core.Repositories
{
    public class BaseRepository
    {
        protected SearchClient _SearchClient;
        protected StarredClient _StarredClient;
    }
}
