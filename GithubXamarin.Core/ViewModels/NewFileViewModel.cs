using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{

    public class NewFileViewModel : BaseViewModel
    {
        #region Properties and Commands

        private readonly IFileDataService _fileDataService;

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                RaisePropertyChanged(() => Path);
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

        private bool _isNew = true;
        public bool IsNew
        {
            get => _isNew;
            set
            {
                _isNew = value;
                RaisePropertyChanged(() => IsNew);
            }
        }


        private ICommand _submitCommand;

        public ICommand SubmitCommand
        {
            get
            {
                _submitCommand = _submitCommand ?? new MvxAsyncCommand(async () => await UpdateorCreateFile());
                return _submitCommand;
            }
        }

        private long _repositoryid;

        private string _sha;

        #endregion


        public NewFileViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IFileDataService fileDataService) : base(githubClientService, messenger, dialogService)
        {
            _fileDataService = fileDataService;
        }

        public async void Init(long repositoryId, string filePath, string content, string sha)
        {
            _repositoryid = repositoryId;
            Path = filePath;
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Creating new file" });

            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(sha))
            {
                Content = content;
                _sha = sha;
                IsEdit = true;
                IsNew = false;
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Editing {Path}" });
            }
        }

        public async Task UpdateorCreateFile()
        {
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            if (IsEdit)
            {
                await _fileDataService.UpdateFile(_repositoryid, Path, Message, Content, _sha,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                await _fileDataService.CreateFile(_repositoryid, Path, _message, Content,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            Close(this);
        }
    }
}
