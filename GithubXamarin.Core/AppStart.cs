using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.SecureStorage;

namespace GithubXamarin.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<MainViewModel>();
        }
    }
}
