using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Plugins.Network.Reachability;
using MvvmCross.Plugins.Network.Rest;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class IssueViewModel : BaseViewModel, IIssueViewModel
    {
        #region Properties and Commands

        private readonly IIssueDataService _issueDataService;

        private Issue _issue;
        public Issue Issue
        {
            get { return _issue;}
            set
            {
                _issue = value;
                RaisePropertyChanged(() => Issue);
            }
        }

        #endregion


        public IssueViewModel(IGithubClientService githubClientService, IIssueDataService issueDataService) : base(githubClientService)
        {
            _issueDataService = issueDataService;
        }

        public async void Init(long repositoryId, int issueNumber)
        {
            if (IsInternetAvailable())
            {
                Issue = await _issueDataService.GetIssueForRepository(repositoryId, issueNumber,
                    GithubClientService.GetAuthorizedGithubClient());
            }
        }
    }
}
