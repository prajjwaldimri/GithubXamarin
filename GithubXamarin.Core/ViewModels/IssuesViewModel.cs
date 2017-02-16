using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using Octokit;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Messages;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{
    public class IssuesViewModel : BaseViewModel, IIssuesViewModel
    {
        #region Properties and Commands

        //DataServices
        private readonly IIssueDataService _issueDataService;

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

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? new MvxAsyncCommand(async () => await Refresh());
                return _refreshCommand;
            }
        }

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

        private long? _repositoryId;

        #endregion

        public IssuesViewModel(IIssueDataService issueDataService, IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _issueDataService = issueDataService;
        }

        public async void Init(long? repositoryId = null)
        {
            _repositoryId = repositoryId;
            await Refresh();
        }

        private void NavigateToIssueView()
        {
            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = Issues[SelectedIssue].Number,
                repositoryId = Issues[SelectedIssue].Repository.Id,
            });
        }

        private async Task Refresh()
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("Or is it?", "Internet is not available");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            try
            {
                if (_repositoryId.HasValue)
                {
                    Issues = await _issueDataService.GetAllIssuesForRepository(_repositoryId.Value,
                        GithubClientService.GetAuthorizedGithubClient());
                    Messenger.Publish(new AppBarHeaderChangeMessage(this)
                    {
                        HeaderTitle = $"Issues for {Issues[0]?.Repository.FullName}"
                    });
                }
                else
                {
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Your Issues" });
                    Issues =
                        await _issueDataService.GetAllIssuesForCurrentUser(
                            GithubClientService.GetAuthorizedGithubClient());
                }
            }
            catch (HttpRequestException)
            {
                await DialogService.ShowDialogASync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
