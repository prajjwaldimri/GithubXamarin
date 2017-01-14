using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
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

        public override void Start()
        {
            base.Start();
        }

        public async void Init(long repositoryId, int issueNumber)
        {
            Issue = await _issueDataService.GetIssueForRepository(repositoryId, issueNumber,
                _githubClientService.GetAuthorizedGithubClient());
        }
    }
}
