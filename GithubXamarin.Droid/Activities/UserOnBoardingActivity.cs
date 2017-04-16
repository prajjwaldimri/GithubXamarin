using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GithubXamarin.Core.ViewModels;
using GithubXamarin.Droid.Services;
using Java.Util;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;

namespace GithubXamarin.Droid.Activities
{
    [Activity(Label = "UserOnBoardingActivity",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/MyTheme",
        Name = "github.droid.activities.UserOnBoardingActivity")]
    public class UserOnBoardingActivity : MvxActivity<UserOnboardingViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.UserOnboarding);

        }
    }
}