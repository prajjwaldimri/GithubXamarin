using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using Octokit;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GithubXamarin.Core.ViewModels
{
    /// <summary>
    /// Shows list of issues in the UI
    /// </summary>
    public class IssuesViewModel : BaseViewModel, IIssuesViewModel
    {
        #region Properties and Commands

        //Commands
        private ICommand _issueClickCommand;

        public ICommand IssueClickCommand
        {
            get
            {
                _issueClickCommand = _issueClickCommand ?? new MvxCommand(NavigateToIssueView);
                return _issueClickCommand;
            }
        }

        //DataServices
        private readonly IIssueDataService _issueDataService;

        //View Properties
        public int SelectedIssue { get; set; }

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

        public async void Init(long? repositoryId = null)
        {
            if (IsInternetAvailable())
            {
                if (repositoryId.HasValue)
                {
                    Issues = await _issueDataService.GetAllIssuesForRepository(repositoryId.Value,
                        GithubClientService.GetAuthorizedGithubClient());
                }
                else
                {
                    Issues =
                        await _issueDataService.GetAllIssuesForCurrentUser(
                            GithubClientService.GetAuthorizedGithubClient());
                }
            }
        }

        private void NavigateToIssueView()
        {
            ShowViewModel<IssueViewModel>(new {issueId = Issues[SelectedIssue].Id});
        }
    }
}
