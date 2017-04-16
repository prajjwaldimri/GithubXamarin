using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{
    public class UserOnboardingViewModel : BaseViewModel
    {

        private ICommand _goToMainViewCommand;
        public ICommand GoToMainViewCommand
        {
            get
            {
                _goToMainViewCommand = _goToMainViewCommand ?? new MvxCommand(GoToMainView);
                return _goToMainViewCommand;
            }
        }


        public UserOnboardingViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
        }

        public void GoToMainView()
        {
            ShowViewModel<MainViewModel>();
        }
    }
}
