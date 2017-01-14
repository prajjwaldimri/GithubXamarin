using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using Octokit;
using System.Collections.ObjectModel;

namespace GithubXamarin.Core.ViewModels
{
    /// <summary>
    /// Shows list of issues in the UI
    /// </summary>
    public class IssuesViewModel : BaseViewModel, IIssuesViewModel
    {
        #region Properties and Commands

        //Commands
        public MvxCommand<Issue> NavigateToIssueCommand { get; set; }

        //DataServices
        private readonly IIssueDataService _issueDataService;

        //View Properties
        private Issue _selectedIssue;
        public Issue SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                RaisePropertyChanged(() => SelectedIssue);
            }
        }

        private ObservableCollection<Issue> _issues;
        public ObservableCollection<Issue> Issues
        {
            get { return _issues; }
            set
            {
                _issues = value;
                RaisePropertyChanged(() => Issues);
            }
        }

        #endregion

        public IssuesViewModel(IIssueDataService issueDataService, IGithubClientService githubClientService) : base(githubClientService)
        {
            _issueDataService = issueDataService;
        }

        public override void Start()
        {
            base.Start();
            NavigateToIssueCommand = new MvxCommand<Issue>(_selectedIssue => ShowViewModel<IssueViewModel>(
                new {issueId = _selectedIssue.Id}));
        }

        public async void Init(long? repositoryId = null)
        {
            if (repositoryId.HasValue)
            {
                Issues = await _issueDataService.GetAllIssuesForRepository(repositoryId.Value,
                    _githubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Issues =
                    await _issueDataService.GetAllIssuesForCurrentUser(_githubClientService.GetAuthorizedGithubClient());
            }
        }
    }
}
