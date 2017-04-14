using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Repository;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Utility;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class IssueDataService : IIssueDataService
    {
        private readonly IIssueRepository _issueRepository;

        public IssueDataService(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<ObservableCollection<Issue>> GetAllOpenIssuesForCurrentUser(GitHubClient authorizedGithubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.GetAllOpenIssuesForCurrentUser(authorizedGithubClient));
        }

        public async Task<ObservableCollection<Issue>> GetAllClosedIssuesForCurrentUser(GitHubClient authorizedGithubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.GetAllClosedIssuesForCurrentUser(authorizedGithubClient));
        }

        public async Task<ObservableCollection<Issue>> GetAllOpenIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.GetAllOpenIssuesForRepository(repositoryId, authorizedGitHubClient));
        }

        public async Task<ObservableCollection<Issue>> GetAllClosedIssuesForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.GetAllClosedIssuesForRepository(repositoryId, authorizedGitHubClient));
        }

        public async Task<Issue> GetIssueForRepository(long repositoryId, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.GetIssueForRepository(repositoryId, issueNumber, authorizedGitHubClient);
        }

        public async Task<ObservableCollection<Issue>> SearchIssues(string searchTerm, GitHubClient gitHubClient)
        {
            return new ObservableCollection<Issue>(await _issueRepository.SearchIssues(searchTerm, gitHubClient));
        }

        public async Task<Issue> GetIssueForRepository(string owner, string repoName, int issueNumber, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.GetIssueForRepository(owner, repoName, issueNumber, authorizedGitHubClient);
        }

        public async Task<Issue> CreateIssue(long repositoryId, NewIssue newIssueDetails, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.CreateIssue(repositoryId, newIssueDetails, authorizedGitHubClient);
        }

        public async Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate updatedIssueDetails, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.UpdateIssue(repositoryId, issueNumber, updatedIssueDetails,
                authorizedGitHubClient);
        }

        public async Task<ObservableCollection<Label>> GetLabelsForRepository(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Label>(await _issueRepository.GetLabelsForRepository(repositoryId, authorizedGitHubClient));
        }

        public async Task AddLabelsToIssue(long repositoryId, int issueNumber, string labels, GitHubClient authorizedGitHubClient)
        {
            await _issueRepository.AddLabelsToIssue(repositoryId, issueNumber, StringToStringArrayConverter.Convert(labels), authorizedGitHubClient);
        }

        public async Task ReplaceLabelsForIssue(long repositoryId, int issueNumber, string labels, GitHubClient authorizedGitHubClient)
        {
            await _issueRepository.ReplaceLabelsForIssue(repositoryId, issueNumber,
                StringToStringArrayConverter.Convert(labels), authorizedGitHubClient);
        }

        #region Issue Comments

        public async Task<ObservableCollection<IssueComment>> GetCommentsForIssue(long repositoryId, int issueNumber, GitHubClient authorizedGithubClient)
        {
            return new ObservableCollection<IssueComment>(await _issueRepository.GetCommentsForIssue(repositoryId, issueNumber, authorizedGithubClient));
        }

        public async Task<IssueComment> GetComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.GetComment(repositoryId, commentId, authorizedGitHubClient);
        }

        public async Task<IssueComment> CreateComment(long repositoryId, int issueNumber, string newComment, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.CreateComment(repositoryId, issueNumber, newComment, authorizedGitHubClient);
        }

        public async Task<IssueComment> UpdateComment(long repositoryId, int issueNumber, string updatedComment, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.UpdateComment(repositoryId, issueNumber, updatedComment,
                authorizedGitHubClient);
        }

        public async Task<bool> DeleteComment(long repositoryId, int commentId, GitHubClient authorizedGitHubClient)
        {
            await _issueRepository.DeleteComment(repositoryId, commentId, authorizedGitHubClient);

            try
            {
                await _issueRepository.GetComment(repositoryId, commentId, authorizedGitHubClient);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Issue Milestones

        public async Task<ObservableCollection<Milestone>> GetMilestonesForRepository(long repoId, GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<Milestone>(await _issueRepository.GetMilestonesForRepository(repoId, authorizedGitHubClient));
        }

        public async Task<Milestone> CreateMilestone(long repoId, NewMilestone newMilestone, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.CreateMilestone(repoId, newMilestone, authorizedGitHubClient);
        }

        public async Task<Milestone> UpdateMilestone(long repoId, int number, MilestoneUpdate milestoneUpdate, GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.UpdateMilestone(repoId, number, milestoneUpdate, authorizedGitHubClient);
        }

        public async Task<bool> DeleteMilestone(long repoId, int number, GitHubClient authorizedGitHubClient)
        {
            await _issueRepository.DeleteMilestone(repoId, number, authorizedGitHubClient);

            try
            {
                await _issueRepository.GetMilestone(repoId, number, authorizedGitHubClient);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Issue Assignees

        public async Task<ObservableCollection<User>> GetAllPossibleAssignees(long repositoryId, GitHubClient authorizedGitHubClient)
        {
            return new ObservableCollection<User>(await _issueRepository.GetAllPossibleAssignees(repositoryId, authorizedGitHubClient));
        }

        public async Task<Issue> AddAssigneesToIssue(string owner, string name, int number, string assignees,
            GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.AddAssigneesToIssue(owner, name, number, new AssigneesUpdate(StringToStringArrayConverter.Convert(assignees)),
                authorizedGitHubClient);
        }

        public async Task<Issue> RemoveAssigneesFromIssue(string owner, string name, int number, string assignees,
            GitHubClient authorizedGitHubClient)
        {
            return await _issueRepository.RemoveAssigneesFromIssue(owner, name, number, new AssigneesUpdate(StringToStringArrayConverter.Convert(assignees)),
                authorizedGitHubClient);
        }

        #endregion
    }
}
