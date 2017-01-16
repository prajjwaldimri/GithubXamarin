using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Plugin.SecureStorage;
using GithubXamarin.Core.ViewModels;
using Android.Widget;

namespace GithubXamarin.Droid.Activities
{
    [Activity(MainLauncher = true,
        Label = "@string/ApplicationName",
        Icon = "@drawable/Icon",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme="@style/MyTheme",
        Name ="github.droid.activities.MainActivity")]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        
        private NavigationView _navigationView;
        private DrawerLayout _drawerLayout;

        internal DrawerLayout DrawerLayout { get { return _drawerLayout; } }

        protected override void OnCreate(Bundle bundle)
        {
            SecureStorageImplementation.StoragePassword = "12345";
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.SetTitle(Resource.String.Empty);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            _navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                //react to click here and swap fragments or navigate
                _drawerLayout.CloseDrawers();
            };

                if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                ViewModel.ShowEvents();
            }
            else
            {
                ViewModel.ShowLogin();
                SetHeader("Login");
            }
        }

        public void SetHeader(string text)
        {
            var toolbarHeader = FindViewById<TextView>(Resource.Id.toolbar_title);
            toolbarHeader.Text = text;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}