using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Support.V7.AppCompat;
using Plugin.SecureStorage;


namespace GithubXamarin.Droid.Activities
{
    [Activity(MainLauncher = true,
        Label = "@string/ApplicationName",
        Icon = "@drawable/Icon",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Name ="github.droid.activities.MainActivity")]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        private FragmentManager _fragmentManager;

        static MainActivity instance = new MainActivity();
        public static MainActivity CurrentActivity => instance;

        public new MainViewModel ViewModel
        {
            get { return (MainViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            SecureStorageImplementation.StoragePassword = "12345";
            base.OnCreate(bundle);
            _fragmentManager = FragmentManager;
            SetContentView(Resource.Layout.Main);
            ViewModel.ShowLogin();
        }
    }
}