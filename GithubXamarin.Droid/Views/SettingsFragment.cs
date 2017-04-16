using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace GithubXamarin.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("githubxamarin.droid.views.SettingsFragment")]
    public class SettingsFragment : MvxFragment<SettingsViewModel>
    {
        private TabLayout _tabLayout;
        private LinearLayout _settingsLinearLayout;
        private LinearLayout _settingsUsersLinearLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            HasOptionsMenu = true;
            return this.BindingInflate(Resource.Layout.SettingsView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _tabLayout = Activity.FindViewById<TabLayout>(Resource.Id.settingsTabLayout);
            _settingsLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.settingsLayout);
            _settingsUsersLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.settingsUsersLayout);

            _tabLayout.TabSelected += async (sender, args) =>
            {
                switch (args.Tab.Text)
                {
                    case "Settings":
                        _settingsLinearLayout.Visibility = ViewStates.Visible;
                        _settingsUsersLinearLayout.Visibility = ViewStates.Gone;
                        break;
                    case "Contributors":
                        _settingsLinearLayout.Visibility = ViewStates.Gone;
                        _settingsUsersLinearLayout.Visibility = ViewStates.Visible;
                        break;
                }
            };

        }
    }
}