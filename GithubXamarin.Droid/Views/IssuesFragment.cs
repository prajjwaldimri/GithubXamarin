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
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace GithubXamarin.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("githubxamarin.droid.views.IssuesFragment")]
    public class IssuesFragment : MvxFragment<IssuesViewModel>
    {
        private TabLayout _tabLayout;
        private LinearLayout _issuesLinearLayout;
        private LinearLayout _closedIssuesLinearLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            HasOptionsMenu = true;
            return this.BindingInflate(Resource.Layout.IssuesView, null);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            await ViewModel.Refresh();

            _tabLayout = Activity.FindViewById<TabLayout>(Resource.Id.issuesTabLayout);
            _issuesLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.issuesLayout);
            _closedIssuesLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.closedIssuesLayout);

            _tabLayout.TabSelected += async (sender, args) =>
            {
                switch (args.Tab.Text)
                {
                    case "Open":
                        _issuesLinearLayout.Visibility = ViewStates.Visible;
                        _closedIssuesLinearLayout.Visibility = ViewStates.Gone;
                        break;
                    case "Closed":
                        _issuesLinearLayout.Visibility = ViewStates.Gone;
                        _closedIssuesLinearLayout.Visibility = ViewStates.Visible;
                        await ViewModel.RefreshClosed();
                        break;
                }
            };
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.issues_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.repositories_refresh:
                    ViewModel.Refresh();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}