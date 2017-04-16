using System.Collections.Generic;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using Octokit;
// ReSharper disable PossibleNullReferenceException

namespace GithubXamarin.Core.Repositories
{
    /// <summary>
    /// https://developer.github.com/v3/issues/
    /// </summary>
    public class IssueRepository : BaseRepository, IIssueRepository
    {
        private IssuesClient _issuesClient;

        public async Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Get(repositoryId, issueNumber);
        }

        public async Task<Issue> GetIssueForRepository(string owner, string repoName, int issueNumber,
            GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Get(owner, repoName, issueNumber);
        }

        public async Task<IEnumerable<Issue>> GetAllOpenIssuesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Issue.GetAllForCurrent();
        }

        public async Task<IEnumerable<Issue>> GetAllClosedIssuesForCurrentUser(GitHubClient authorizedGitHubClient)
        {
            return await authorizedGitHubClient.Issue.GetAllForCurrent(new IssueRequest { State = ItemStateFilter.Closed });
        }

        public async Task<IEnumerable<Issue>> GetAllOpenIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.GetAllForRepository(repositoryId);
        }

        public async Task<IEnumerable<Issue>> GetAllClosedIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.GetAllForRepository(repositoryId, new RepositoryIssueRequest { State = ItemStateFilter.Closed });
        }

        public async Task<IEnumerable<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            if (_SearchClient == null)
            {
                _SearchClient = new SearchClient(new ApiConnection(gitHubClient.Connection));
            }
            var searchResult = await _SearchClient.SearchIssues(new SearchIssuesRequest(searchTerm));
            return searchResult.Items;
        }

        public async Task<Issue> CreateIssue(long repositoryId, NewIssue newIssueDetails, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Create(repositoryId, newIssueDetails);
        }

        public async Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate updatedIssueDetails,
            GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Update(repositoryId, issueNumber, updatedIssueDetails);
        }

        public async Task<IEnumerable<Label>> GetLabelsForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Labels.GetAllForRepository(repositoryId);
        }

        public async Task AddLabelsToIssue(long repositoryId, int issueNumber, string[] labels, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            await _issuesClient.Labels.AddToIssue(repositoryId, issueNumber, labels);
        }

        public async Task ReplaceLabelsForIssue(long repositoryId, int issueNumber, string[] labels, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            await _issuesClient.Labels.ReplaceAllForIssue(repositoryId, issueNumber, labels);
        }

        #region IssueComments https://developer.github.com/v3/issues/comments/

        public async Task<IEnumerable<IssueComment>> GetCommentsForIssue(long repositoryId, int issueNumber, GitHubClient authorizedGithubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGithubClient.Connection));
            }
            return await _issuesClient.Comment.GetAllForIssue(repositoryId, issueNumber);
        }

        public async Task<IssueComment> GetComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Comment.Get(repositoryId, commentId);
        }

        public async Task<IssueComment> CreateComment(long repositoryId, int issueNumber, string newComment, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Comment.Create(repositoryId, issueNumber, newComment);
        }

        public async Task<IssueComment> UpdateComment(long repositoryId, int issueNumber, string updatedComment, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Comment.Update(repositoryId, issueNumber, updatedComment);
        }

        public async Task DeleteComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            await _issuesClient.Comment.Delete(repositoryId, commentId);
        }

        #endregion

        #region IssueMileStones https://developer.github.com/v3/issues/milestones

        public async Task<IEnumerable<Milestone>> GetMilestonesForRepository(long repoId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Milestone.GetAllForRepository(repoId);
        }

        public async Task<Milestone> GetMilestone(long repoId, int number, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Milestone.Get(repoId, number);
        }

        public async Task<Milestone> CreateMilestone(long repoId, NewMilestone newMilestone, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Milestone.Create(repoId, newMilestone);
        }

        public async Task<Milestone> UpdateMilestone(long repoId, int number, MilestoneUpdate milestoneUpdate, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Milestone.Update(repoId, number, milestoneUpdate);
        }

        public async Task DeleteMilestone(long repoId, int number, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            await _issuesClient.Milestone.Delete(repoId, number);
        }

        #endregion

        #region IssueAssignees https://developer.github.com/v3/issues/assignees

        public async Task<IEnumerable<User>> GetAllPossibleAssignees(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Assignee.GetAllForRepository(repositoryId);
        }

        public async Task<Issue> AddAssigneesToIssue(string owner, string name, int number, AssigneesUpdate assigneesUpdate, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Assignee.AddAssignees(owner, name, number, assigneesUpdate);
        }

        public async Task<Issue> RemoveAssigneesFromIssue(string owner, string name, int number, AssigneesUpdate assigneesUpdate, GitHubClient authorizedGitHubClient)
        {
            if (_issuesClient == null)
            {
                _issuesClient = new IssuesClient(new ApiConnection(authorizedGitHubClient.Connection));
            }
            return await _issuesClient.Assignee.RemoveAssignees(owner, name, number, assigneesUpdate);
        }

        #endregion


    }
}
