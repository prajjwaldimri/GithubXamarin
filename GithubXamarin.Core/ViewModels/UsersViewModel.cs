using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Model;
using MvvmCross.Core.ViewModels;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class UsersViewModel : BaseViewModel, IUsersViewModel
    {
        #region Commands and Properties

        private readonly IUserDataService _userDataService;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users;}
            set
            {
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public int SelectedIndex { get; set; }

        private ICommand _userClickCommand;
        public ICommand UserClickCommand
        {
            get
            {
                _userClickCommand = _userClickCommand ?? new MvxCommand(NavigateToUserView);
                return _userClickCommand;
            }
        }

        #endregion

        public UsersViewModel(IGithubClientService githubClientService, IUserDataService userDataService) : base(githubClientService)
        {
            _userDataService = userDataService;
        }

        public async void Init(long repositoryId, UsersTypeEnumeration? usersType = null)
        {
            if (usersType != null)
            {
                switch (usersType)
                {
                    case UsersTypeEnumeration.Stargazers:
                        Users = await _userDataService.GetStargazersForRepository(repositoryId, _githubClientService.GetAuthorizedGithubClient());
                        break;
                    case UsersTypeEnumeration.Collaborators:
                        Users = await _userDataService.GetCollaboratorsForRepository(repositoryId,
                            _githubClientService.GetAuthorizedGithubClient());
                        break;
                    default:
                        Users = await _userDataService.GetCollaboratorsForRepository(repositoryId,
                            _githubClientService.GetAuthorizedGithubClient());
                        break;
                }
            }
            else
            {
                Users = await _userDataService.GetCollaboratorsForRepository(repositoryId, 
                    _githubClientService.GetAuthorizedGithubClient());
            }
        }

        public void NavigateToUserView()
        {
            var user = Users?[SelectedIndex];
            if (user != null)
                ShowViewModel<UserViewModel>(new {userLogin = user.Login});
        }
    }
}
