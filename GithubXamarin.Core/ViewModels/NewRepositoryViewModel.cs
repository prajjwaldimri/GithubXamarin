using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class NewRepositoryViewModel : BaseViewModel
    {
        #region Properties and Commands

        private readonly IRepoDataService _repoDataService;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private string _homePage;
        public string HomePage
        {
            get => _homePage;
            set
            {
                _homePage = value;
                RaisePropertyChanged(() => HomePage);
            }
        }

        private bool _isPrivate;
        public bool IsPrivate
        {
            get => _isPrivate;
            set
            {
                _isPrivate = value;
                RaisePropertyChanged(() => IsPrivate);
            }
        }

        private bool _hasIssues = true;
        public bool HasIssues
        {
            get => _hasIssues;
            set
            {
                _hasIssues = value;
                RaisePropertyChanged(() => HasIssues);
            }
        }

        private int _repositoryStatusSelectedIndex = 1;
        public int RepositoryStatusSelectedIndex
        {
            get => _repositoryStatusSelectedIndex;
            set
            {
                _repositoryStatusSelectedIndex = value;
                switch (value)
                {
                    case 0:
                        IsPrivate = true;
                        break;
                    case 1:
                        IsPrivate = false;
                        break;
                }
                RaisePropertyChanged(() => RepositoryStatusSelectedIndex);
            }
        }

        private bool _hasWiki = true;
        public bool HasWiki
        {
            get => _hasWiki;
            set
            {
                _hasWiki = value;
                RaisePropertyChanged(() => HasWiki);
            }
        }

        private bool _createWithReadme;
        public bool CreateWithReadme
        {
            get => _createWithReadme;
            set
            {
                _createWithReadme = value;
                RaisePropertyChanged(() => CreateWithReadme);
            }
        }

        private string _gitignoreTemplate;
        public string GitignoreTemplate
        {
            get => _gitignoreTemplate;
            set
            {
                _gitignoreTemplate = value;
                RaisePropertyChanged(() => GitignoreTemplate);
            }
        }

        private string _licenseTemplate;
        public string LicenseTemplate
        {
            get => _licenseTemplate;
            set
            {
                _licenseTemplate = value;
                RaisePropertyChanged(() => LicenseTemplate);
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

        private bool _isNew;
        public bool IsNew
        {
            get => _isNew;
            set
            {
                _isNew = value;
                RaisePropertyChanged(() => IsNew);
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

        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                _submitCommand = _submitCommand ?? new MvxAsyncCommand(CreateOrEditRepo);
                return _submitCommand;
            }
        }

        public List<string> RepositoryStateCategories { get; } = new List<string>()
        {
            "Private", "Public"
        };

        #endregion


        public NewRepositoryViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IRepoDataService repoDataService) : base(githubClientService, messenger, dialogService)
        {
            _repoDataService = repoDataService;
        }

        public async void Init(long repositoryId, string name, string description, string homePage, bool isPrivate, bool hasIssues, bool hasWiki)
        {
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Creating new repository" });

            if (!(string.IsNullOrWhiteSpace(name)))
            {
                RepositoryId = repositoryId;
                Name = name;
                Description = description;
                HomePage = homePage;
                IsPrivate = isPrivate;
                HasIssues = hasIssues;
                HasWiki = HasWiki;
                IsEdit = true;
                IsNew = false;
            }
            else
            {
                IsEdit = false;
                IsNew = true;
            }
            if (IsPrivate)
            {
                RepositoryStatusSelectedIndex = 0;
            }
        }

        private async Task CreateOrEditRepo()
        {
            if (!(await IsInternetAvailable())) { await DialogService.ShowSimpleDialogAsync("Can't fetch the error message from the internet.", "No Internet"); return; }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });


            if (IsEdit)
            {
                await EditRepo();
            }
            else
            {
                await CreateRepo();
            }


            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async Task CreateRepo()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return;
            }

            var createdRepo = await _repoDataService.CreateRepository(new NewRepository(Name)
            {
                GitignoreTemplate = GitignoreTemplate,
                AutoInit = CreateWithReadme,
                Description = Description,
                HasIssues = HasIssues,
                HasWiki = HasWiki,
                Homepage = HomePage,
                Private = IsPrivate,
                LicenseTemplate = LicenseTemplate
            },
                GithubClientService.GetAuthorizedGithubClient());

            ShowViewModel<RepositoryViewModel>(new { repositoryId = createdRepo.Id });
        }

        private async Task EditRepo()
        {
            var updatedRepo = await _repoDataService.UpdateRepository(RepositoryId, new RepositoryUpdate(Name)
            {
                Description = Description,
                HasIssues = HasIssues,
                HasWiki = HasWiki,
                Homepage = HomePage,
                Private = IsPrivate
            }, GithubClientService.GetAuthorizedGithubClient());

            ShowViewModel<RepositoryViewModel>(new { repositoryId = updatedRepo.Id });
        }
    }
}
