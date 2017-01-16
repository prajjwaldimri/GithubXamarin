using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;

namespace GithubXamarin.Core.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        public MainViewModel(IGithubClientService githubClientService) : base(githubClientService)
        {
        }

        public void ShowEvents()
        {
            ShowViewModel<EventsViewModel>();
        }

        public void ShowLogin()
        {
            ShowViewModel<LoginViewModel>();
        }
    }
}
