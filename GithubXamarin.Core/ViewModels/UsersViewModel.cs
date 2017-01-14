using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Model;
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

        #endregion


        public UsersViewModel(IGithubClientService githubClientService, IUserDataService userDataService) : base(githubClientService)
        {
            _userDataService = userDataService;
        }

        public override void Start()
        {
            base.Start();
        }

        public async void Init(long repositoryId, UsersTypeEnumeration usersType)
        {
            if (usersType == UsersTypeEnumeration.Stargazers)
            {
                Users = await _userDataService.GetStargazersForRepository(repositoryId, _githubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Users = await _userDataService.GetCollaboratorsForRepository(repositoryId, _githubClientService.GetAuthorizedGithubClient());
            }
        }
    }
}
