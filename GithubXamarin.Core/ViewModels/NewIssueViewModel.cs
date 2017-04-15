using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class NewIssueViewModel : BaseViewModel
    {
        #region Properties and Commands

        private readonly IIssueDataService _issueDataService;
        private readonly IRepoDataService _repoDataService;

        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                _submitCommand = _submitCommand ?? new MvxAsyncCommand(CreateOrUpdateIssue);
                return _submitCommand;
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _body;
        public string Body
        {
            get => _body;
            set
            {
                _body = value;
                RaisePropertyChanged(() => Body);
            }
        }

        private string _labels;
        public string Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                RaisePropertyChanged(() => Labels);
            }
        }

        private ObservableCollection<Label> _availableLabels;
        public ObservableCollection<Label> AvailableLabels
        {
            get => _availableLabels;
            set
            {
                _availableLabels = value;
                RaisePropertyChanged(() => AvailableLabels);
            }
        }

        private string _assignees;
        public string Assignees
        {
            get => _assignees;
            set
            {
                _assignees = value;
                RaisePropertyChanged(() => Assignees);
            }
        }

        private ObservableCollection<User> _availableAssignees;
        public ObservableCollection<User> AvailableAssignees
        {
            get => _availableAssignees;
            set
            {
                _availableAssignees = value;
                RaisePropertyChanged(() => AvailableAssignees);
            }
        }

        private ObservableCollection<Milestone> _milestones;
        public ObservableCollection<Milestone> Milestones
        {
            get => _milestones;
            set
            {
                _milestones = value;
                RaisePropertyChanged(() => Milestones);
            }
        }

        private int _selectedMilestoneIndex;
        public int SelectedMilestoneIndex
        {
            get => _selectedMilestoneIndex;
            set
            {
                _selectedMilestoneIndex = value;
                RaisePropertyChanged(() => SelectedMilestoneIndex);
            }
        }

        private long _repositoryId;
        public long RepositoryId
        {
            get => _repositoryId;
            set
            {
                _repositoryId = value;
                RaisePropertyChanged(() => RepositoryId);
            }
        }

        private int _issueNumber;
        public int IssueNumber
        {
            get => _issueNumber;
            set
            {
                _issueNumber = value;
                RaisePropertyChanged(() => IssueNumber);
            }
        }

        private int _issueStateSelectedIndex;
        public int IssueStateSelectedIndex
        {
            get => _issueStateSelectedIndex;
            set
            {
                _issueStateSelectedIndex = value;
                switch (value)
                {
                    case 0:
                        IssueItemState = ItemState.Open;
                        break;
                    case 1:
                        IssueItemState = ItemState.Closed;
                        break;
                }
                RaisePropertyChanged(() => IssueStateSelectedIndex);
            }
        }

        private ItemState _issueItemState;
        public ItemState IssueItemState
        {
            get => _issueItemState;
            set
            {
                _issueItemState = value;
                RaisePropertyChanged(() => IssueItemState);
            }
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                _isEdit = value;
                RaisePropertyChanged(() => IsEdit);
            }
        }

        public List<string> IssueStateCategories { get; } = new List<string>()
        {
            "Open", "Closed"
        };

        private List<string> _milestoneNamesList = new List<string>();
        public List<string> MilestoneNamesList
        {
            get => _milestoneNamesList;
            set => _milestoneNamesList = value;
        }


        private string _milestone;
        private Repository _repository;
        private string _originalAssignees;

        #endregion 

        public NewIssueViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IIssueDataService issueDataService, IRepoDataService repoDataService) : base(githubClientService, messenger, dialogService)
        {
            _issueDataService = issueDataService;
            _repoDataService = repoDataService;
        }

        public async void Init(long repositoryId, int issueNumber, string issueTitle = null, string issueBody = null, string labels = null, string assignees = null, string milestone = null)
        {
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            RepositoryId = repositoryId;
            IsEdit = false;
            IssueNumber = issueNumber;
            _milestone = milestone;
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Creating a new issue" });
            if (!(string.IsNullOrWhiteSpace(issueTitle)))
            {
                Title = issueTitle;
                Body = issueBody;
                Labels = labels;
                Assignees = assignees;
                _originalAssignees = assignees;
                IsEdit = true;
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Editing {Title}" });
            }
            await GetMilestones();
            await GetRepositoryDetails();

            AvailableLabels =
                await _issueDataService.GetLabelsForRepository(RepositoryId, GithubClientService.GetAuthorizedGithubClient());

            AvailableAssignees =
                await _issueDataService.GetAllPossibleAssignees(repositoryId,
                    GithubClientService.GetAuthorizedGithubClient());

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async Task CreateOrUpdateIssue()
        {
            if (!(await IsInternetAvailable()))
            {
                await DialogService.ShowSimpleDialogAsync("I am a very PC person! I don't like working on laptops", "Internet not available");
                return;
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (IsEdit)
            {
                await UpdateIssue();
            }
            else
            {
                await CreateIssue();
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async Task CreateIssue()
        {
            if (string.IsNullOrWhiteSpace(Title)) { return; }

            int? milestoneNumber = null;
            if (SelectedMilestoneIndex > 0)
            {
                milestoneNumber = Milestones[SelectedMilestoneIndex].Number;
            }

            var createdIssue = await _issueDataService.CreateIssue(RepositoryId, new NewIssue(Title)
            {
                Body = Body,
                Milestone = milestoneNumber
            }, GithubClientService.GetAuthorizedGithubClient());

            if (Labels != null)
            {
                await _issueDataService.AddLabelsToIssue(RepositoryId, createdIssue.Number,
                    Labels, GithubClientService.GetAuthorizedGithubClient());
            }

            if (Assignees != null)
            {
                await _issueDataService.AddAssigneesToIssue(_repository.Owner.Login, _repository.Name, _issueNumber,
                    Assignees, GithubClientService.GetAuthorizedGithubClient());
            }

            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = createdIssue.Number,
                repositoryId = RepositoryId,
            });
        }

        private async Task UpdateIssue()
        {
            if (string.IsNullOrWhiteSpace(Title) || IssueNumber == null) { return; }

            int? milestoneNumber = null;
            if (SelectedMilestoneIndex > 0)
            {
                try
                {
                    milestoneNumber = Milestones[SelectedMilestoneIndex].Number;
                }
                //Android Problems :P
                catch (ArgumentOutOfRangeException)
                {
                    milestoneNumber = Milestones[SelectedMilestoneIndex - 1].Number;
                }
            }

            var createdIssue = await _issueDataService.UpdateIssue(RepositoryId, IssueNumber, new IssueUpdate()
            {
                Title = Title,
                Body = Body,
                State = IssueItemState,
                Milestone = milestoneNumber
            }, GithubClientService.GetAuthorizedGithubClient());

            if (Labels != null)
            {
                await _issueDataService.ReplaceLabelsForIssue(RepositoryId, createdIssue.Number,
                    Labels, GithubClientService.GetAuthorizedGithubClient());
            }

            if (string.IsNullOrWhiteSpace(Assignees))
            {
                await _issueDataService.RemoveAssigneesFromIssue(_repository.Owner.Login, _repository.Name, _issueNumber, _originalAssignees, GithubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                await _issueDataService.AddAssigneesToIssue(_repository.Owner.Login, _repository.Name, _issueNumber,
                    Assignees, GithubClientService.GetAuthorizedGithubClient());
            }

            ShowViewModel<IssueViewModel>(new
            {
                issueNumber = createdIssue.Number,
                repositoryId = RepositoryId,
            });
        }

        private async Task GetMilestones()
        {
            Milestones = await _issueDataService.GetMilestonesForRepository(RepositoryId,
                GithubClientService.GetAuthorizedGithubClient());

            MilestoneNamesList = new List<string>(Milestones.Count);
            MilestoneNamesList.Add("No Milestone");

            for (var i = 0; i < Milestones.Count; i++)
            {
                MilestoneNamesList.Add(Milestones[i].Title);
                if (Milestones[i].Title.Equals(_milestone))
                {
                    SelectedMilestoneIndex = i;
                }
            }
            RaisePropertyChanged(() => MilestoneNamesList);
        }

        private async Task GetRepositoryDetails()
        {
            _repository =
                await _repoDataService.GetRepository(RepositoryId, GithubClientService.GetAuthorizedGithubClient());
        }
    }
}
