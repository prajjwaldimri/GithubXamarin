using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Shared.Presenter;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using System.Reflection;
using System.Collections.Generic;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Droid.Services;
using Plugin.SecureStorage;

namespace GithubXamarin.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
            SecureStorageImplementation.StoragePassword = "12345";
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        //Android-Specific IOC Registrations
        protected override void InitializeIoC()
        {
            base.InitializeIoC();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            Mvx.ConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.ConstructAndRegisterSingleton<IShareService, ShareService>();
            Mvx.ConstructAndRegisterSingleton<IUpdateService, UpdateService>();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var mvxFragmentsPresenter = new MvxFragmentsPresenter(AndroidViewAssemblies);
            Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(mvxFragmentsPresenter);
            return mvxFragmentsPresenter;
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(Android.Support.V7.Widget.Toolbar).Assembly,
            typeof(Android.Support.V4.Widget.DrawerLayout).Assembly,
            typeof(Android.Support.V4.View.ViewPager).Assembly,
        };
    }
}