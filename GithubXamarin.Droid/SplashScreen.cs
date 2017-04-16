using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace GithubXamarin.Droid
{
    [Activity(MainLauncher = true,
        Label = "@string/ApplicationName",
        Icon = "@drawable/ic_launcher",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/MyTheme.Splash",
        NoHistory = true,
        Name = "github.droid.activities.SplashActivity")]
    public class SplashScreen : MvxSplashScreenActivity
    {

    }
}