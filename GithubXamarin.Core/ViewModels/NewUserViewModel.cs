using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    /// <summary>
    /// https://developer.github.com/v3/users
    /// </summary>
    public class NewUserViewModel : BaseViewModel
    {
        #region Properties and Commands

        private readonly IUserDataService _userDataService;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                RaisePropertyChanged(() => Name);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        private string _blogUrl;
        public string BlogUrl
        {
            get { return _blogUrl; }
            set
            {
                _blogUrl = value; 
                RaisePropertyChanged(() => BlogUrl);
            }
        }

        private string _company;
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                RaisePropertyChanged(() => Company);
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value; 
                RaisePropertyChanged(() => Location);
            }
        }

        private bool _hireable;
        public bool Hireable
        {
            get { return _hireable; }
            set
            {
                _hireable = value;
                RaisePropertyChanged(() => Hireable);
            }
        }

        private string _bio;
        public string Bio
        {
            get { return _bio; }
            set
            {
                _bio = value;
                RaisePropertyChanged(() => Bio);
            }
        }

        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                _submitCommand = _submitCommand ?? new MvxAsyncCommand(UpdateUser);
                return _submitCommand;
            }
        }

        #endregion

        public NewUserViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IUserDataService userDataService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
        }

        public void Init(string name, string email, string blog, string company, string location, bool hireable, string bio)
        {
            Name = name;
            Email = email;
            BlogUrl = blog;
            Company = company;
            Location = location;
            Hireable = hireable;
            Bio = bio;
        }

        private async Task UpdateUser()
        {
            if (!!(await IsInternetAvailable())) return;
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });
            var updatedUser = await _userDataService.UpdateUser(new UserUpdate()
                {
                    Bio = Bio,
                    Blog = BlogUrl,
                    Company = Company,
                    Email = Email,
                    Hireable = Hireable,
                    Location = Location,
                    Name = Name
                },
                GithubClientService.GetAuthorizedGithubClient());
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
            Close(this);
        }
    }
}
