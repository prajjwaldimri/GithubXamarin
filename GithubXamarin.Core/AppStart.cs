using System.IO;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using Plugin.SecureStorage;

namespace GithubXamarin.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            try
            {
                if (CrossSecureStorage.Current.HasKey("UserOnBoard"))
                {
                    ShowViewModel<MainViewModel>();
                }
                else
                {
                    ShowViewModel<UserOnboardingViewModel>();
                    CrossSecureStorage.Current.SetValue("UserOnBoard", "Boarded");
                }
            }
            catch (FileNotFoundException)
            {
                ShowViewModel<MainViewModel>();
            }
        }
    }
}
