using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{
    public class FileViewModel : BaseViewModel, IFileViewModel
    {
        #region Properties and Commands

        private readonly IFileDataService _fileDataService;
        private readonly IShareService _shareService;

        private string _fileContent;
        public string FileContent
        {
            get => _fileContent;
            set { _fileContent = value; RaisePropertyChanged(() => FileContent); }
        }

        private FileTypeEnumeration _fileType;

        private string _filePath;

        private string _fileSha;

        private long _repositoryId;

        private bool _isReadme;
        public bool IsReadme
        {
            get => _isReadme;
            set
            {
                _isReadme = value;
                RaisePropertyChanged(() => IsReadme);
            }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                _editCommand = _editCommand ?? new MvxCommand(GoToNewFileView);
                return _editCommand;
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

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                _shareCommand = _shareCommand ?? new MvxAsyncCommand(async () => await ShareFile());
                return _shareCommand;
            }
        }

        #endregion

        public FileViewModel(IFileDataService fileDataService, IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IShareService shareService) : base(githubClientService, messenger, dialogService)
        {
            _fileDataService = fileDataService;
            _shareService = shareService;
        }

        public async void Init(FileTypeEnumeration fileType, long repositoryId, string filePath = null)
        {
            _fileType = fileType;
            _filePath = filePath;
            _repositoryId = repositoryId;
            await Refresh();
        }

        public async Task ShareFile()
        {
            await _shareService.ShareLinkAsync(new Uri(_filePath), _filePath);
        }

        public void GoToNewFileView()
        {
            if (string.IsNullOrWhiteSpace(_fileSha)) return;

            ShowViewModel<NewFileViewModel>(new
            {
                repositoryId = _repositoryId,
                filePath = _filePath,
                content = FileContent,
                sha = _fileSha
            });
        }

        public async Task Refresh()
        {
            if (!(await IsInternetAvailable()))
            {
                await DialogService.ShowSimpleDialogAsync("I got nothing here.", "Internet Not Available!");
                return;
            }

            switch (_fileType)
            {
                case FileTypeEnumeration.Readme:

                    IsReadme = true;
                    var readme = await _fileDataService.GetReadme(_repositoryId,
                        GithubClientService.GetAuthorizedGithubClient());
                    FileContent = readme.Content;
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"{readme.Name}" });
                    break;

                case FileTypeEnumeration.Encoded:

                    break;

                case FileTypeEnumeration.Nonencoded:

                    IsReadme = false;
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"{_filePath}" });

                    var file = await _fileDataService.GetFile(_repositoryId, _filePath,
                        GithubClientService.GetAuthorizedGithubClient());
                    _fileSha = file.Sha;
                    FileContent = file.Content;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
