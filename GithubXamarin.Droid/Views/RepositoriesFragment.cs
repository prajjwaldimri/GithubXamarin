using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;


namespace GithubXamarin.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("githubxamarin.droid.views.RepositoriesFragment")]
    public class RepositoriesFragment : MvxFragment<RepositoriesViewModel>
    {
        private TabLayout _tabLayout;
        private LinearLayout _repoLinearLayout;
        private LinearLayout _starredRepoLinearLayout;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            HasOptionsMenu = true;
            return this.BindingInflate(Resource.Layout.RepositoriesView, null);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            await ViewModel.Refresh();
            _tabLayout = Activity.FindViewById<TabLayout>(Resource.Id.repoTabLayout);
            _repoLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.repoLayout);
            _starredRepoLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.starredRepoLayout);

            _tabLayout.TabSelected += async (sender, args) =>
            {
                switch (args.Tab.Text)
                {
                    case "Yours":
                        _repoLinearLayout.Visibility = ViewStates.Visible;
                        _starredRepoLinearLayout.Visibility = ViewStates.Gone;
                        break;
                    case "Starred":
                        _repoLinearLayout.Visibility = ViewStates.Gone;
                        _starredRepoLinearLayout.Visibility = ViewStates.Visible;
                        await ViewModel.RefreshStarred();
                        break;
                }
            };
        }
        
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.repositories_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.repositories_refresh:
                    ViewModel.Refresh();
                    break;
                case Resource.Id.repositories_add:
                    ViewModel.GoToNewRepositoryView();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}