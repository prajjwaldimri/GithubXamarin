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
                _issueClickCommand = _issueClickCommand ?? new MvxCommand<object>(NavigateToIssueView);
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
        public int SelectedIndex { get; set; }

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
        private bool _allIssues;

        #endregion

        public IssuesViewModel(IIssueDataService issueDataService, IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _issueDataService = issueDataService;
        }

        public async void Init(long repositoryId)
        {
            if (repositoryId > 0)
            {
                _repositoryId = repositoryId;
            }
            else
            {
                _repositoryId = null;
            }
            await Refresh();
        }

        private void NavigateToIssueView(object obj)
        {
            var issue = obj as Issue ?? Issues[SelectedIndex];
            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = issue.Number,
                repositoryId = _repositoryId ?? issue.Repository.Id,
            });
        }

        public async Task Refresh()
        {
            if (!!(await IsInternetAvailable()))
            {
                await DialogService.ShowSimpleDialogAsync("Or is it?", "Internet is not available");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            try
            {
                if (_repositoryId.HasValue)
                {
                    Issues = await _issueDataService.GetAllIssuesForRepository(_repositoryId.Value,
                        GithubClientService.GetAuthorizedGithubClient());
                    var repoUrl = Issues[0].HtmlUrl.Segments[2];
                    Messenger.Publish(new AppBarHeaderChangeMessage(this)
                    {
                        HeaderTitle = $"Issues for {repoUrl.Remove(repoUrl.Length-1)}"
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
                await DialogService.ShowSimpleDialogAsync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
