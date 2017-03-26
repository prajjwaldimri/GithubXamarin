using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Plugin.SecureStorage;
using GithubXamarin.Core.ViewModels;
using GithubXamarin.Droid.Services;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace GithubXamarin.Droid.Activities
{
    [Activity(Label = "@string/ApplicationName",
        Icon = "@drawable/ic_launcher",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/MyTheme",
        Name = "github.droid.activities.MainActivity")]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        private ActionBarDrawerToggle _drawerToggle;
        private NavigationView _navigationView;
        private Toolbar _toolbar;
        private NavigationView.IOnNavigationItemSelectedListener _selectDrawerItem;
        private SearchView _searchView;

        private DrawerLayout DrawerLayout { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            SecureStorageImplementation.StoragePassword = "12345";
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            
            //Coupling Toolbar and Drawer
            _toolbar.SetTitle(Resource.String.Empty);
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            _navigationView.NavigationItemSelected += _navigationView_NavigationItemSelected;

            //Animating Hamburger Icon. 
            _drawerToggle = setupDrawerToggle();
            DrawerLayout.AddDrawerListener(_drawerToggle);

            ViewModel.LoadFragments();

            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                ScheduleAlarm();
            }
            SetStatusBarTheme();
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
                case (Resource.Id.nav_settings):
                    itemId = 5;
                    break;
            }
            DrawerLayout.CloseDrawers();
            ViewModel.NavigateToViewModel(itemId);
        }

        //Reference: https://github.com/codepath/android_guides/wiki/Fragment-Navigation-Drawer#animate-the-hamburger-icon
        private ActionBarDrawerToggle setupDrawerToggle()
        {
            // NOTE: Make sure you pass in a valid toolbar reference.  ActionBarDrawToggle() does not require it
            // and will not render the hamburger icon without it.  
            return new ActionBarDrawerToggle(this, DrawerLayout, _toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.item_search, menu);
            var item = menu.FindItem(Resource.Id.action_search);
            var searchView = MenuItemCompat.GetActionView(item);
            _searchView = searchView.JavaCast<SearchView>();

            _searchView.QueryTextSubmit += (sender, args) =>
            {
                ViewModel.GoToSearchViewModel(args.Query);
                args.Handled = true;
            };
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            DrawerLayout.CloseDrawers();
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            //Pass any configuration change to drawer toggles
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        private void ScheduleAlarm()
        {
            var intent = new Intent(ApplicationContext.ApplicationContext, typeof(AlarmBroadcastReciever));
            var pendingIntent = PendingIntent.GetBroadcast(this, AlarmBroadcastReciever.RequestCode, intent,
                PendingIntentFlags.UpdateCurrent);
            var alarm = (AlarmManager) this.GetSystemService(Context.AlarmService);
            alarm.SetInexactRepeating(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis(), AlarmManager.IntervalFifteenMinutes, pendingIntent);
        }

        private void SetStatusBarTheme()
        {
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
        }
    }
}