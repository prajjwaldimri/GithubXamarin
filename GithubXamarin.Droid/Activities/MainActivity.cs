using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Plugin.SecureStorage;
using GithubXamarin.Core.ViewModels;
using Android.Widget;
using Auth0.SDK;

namespace GithubXamarin.Droid.Activities
{
    [Activity(MainLauncher = true,
        Label = "@string/ApplicationName",
        Icon = "@drawable/Icon",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/MyTheme",
        Name = "github.droid.activities.MainActivity")]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        //**Auth0**
        private Auth0Client auth0Client = new Auth0Client("prajjwaldimri.auth0.com", "YPkuBnH4kb09eqTYOa9Enz2FPqzULiLZ");

        private ActionBarDrawerToggle drawerToggle;
        private NavigationView _navigationView;
        private DrawerLayout _drawerLayout;
        private Toolbar toolbar;
        private NavigationView.IOnNavigationItemSelectedListener selectDrawerItem;

        internal DrawerLayout DrawerLayout { get { return _drawerLayout; } }

        protected override void OnCreate(Bundle bundle)
        {
            SecureStorageImplementation.StoragePassword = "12345";
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //Coupling Toolbar and Drawer
            toolbar.SetTitle(Resource.String.Empty);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            _navigationView.NavigationItemSelected += _navigationView_NavigationItemSelected;

            //Animating Hamburger Icon. 
            drawerToggle = setupDrawerToggle();
            _drawerLayout.AddDrawerListener(drawerToggle);

            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                SetToolBarHeader("Your Events");
                ViewModel.ShowEvents();
            }
            else
            {
                SetToolBarHeader("Login To Github");
                ViewModel.ShowLogin();
            }
        }

        private void _navigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            e.MenuItem.SetChecked(true);
            int itemId = 0;
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_home):
                    break;
                case (Resource.Id.nav_notifications):
                    itemId = 1;
                    break;
                case (Resource.Id.nav_repositories):
                    itemId = 2;
                    break;
                case (Resource.Id.nav_issues):
                    itemId = 3;
                    break;
                case (Resource.Id.nav_gists):
                    itemId = 4;
                    break;
            }

            _drawerLayout.CloseDrawers();
            ViewModel.ShowViewModelByNavigationDrawerMenuItem(itemId);
        }

        public void SetToolBarHeader(string text)
        {
            var toolbarHeader = FindViewById<TextView>(Resource.Id.toolbar_title);
            toolbarHeader.Text = text;
        }

        //Reference: https://github.com/codepath/android_guides/wiki/Fragment-Navigation-Drawer#animate-the-hamburger-icon
        private ActionBarDrawerToggle setupDrawerToggle()
        {
            // NOTE: Make sure you pass in a valid toolbar reference.  ActionBarDrawToggle() does not require it
            // and will not render the hamburger icon without it.  
            return new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
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

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            //Pass any configuration change to drawer toggles
            drawerToggle.OnConfigurationChanged(newConfig);
        }
    }
}