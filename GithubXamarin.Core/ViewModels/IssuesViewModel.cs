using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using Octokit;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GithubXamarin.Core.Messages;
using MvvmCross.Plugins.Messenger;

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

        public IssuesViewModel(IIssueDataService issueDataService, IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _issueDataService = issueDataService;
        }

        public async void Init(long? repositoryId = null)
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("Or is it?", "Internet is not available");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
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
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private void NavigateToIssueView()
        {
            ShowViewModel<IssueViewModel>(new { issueId = Issues[SelectedIssue].Id });
        }
    }
}
