using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;

namespace GithubXamarin.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        public LoginViewModel(IGithubClientService githubClientService) : base(githubClientService)
        {
        }
    }
}
