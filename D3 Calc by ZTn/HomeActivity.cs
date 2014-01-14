using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using System.Reflection;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/HomeActivityLabel", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : FragmentActivity, NavigationDrawerFragment.ICallbacks
    {
        private NavigationDrawerFragment drawerFragment;
        private string beforeDrawerTitle;

        #region >> FragmentActivity

        /// <inheritdoc/>
        public override void OnBackPressed()
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnBackPressed();

            Finish();
        }

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NavigationDrawerFragmentContainer);

            if (savedInstanceState == null)
            {
                ConfigureNavigationDrawer();
            }
        }

        /// <inheritdoc/>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            MenuInflater.Inflate(Resource.Menu.Settings, menu);

            // Only show items in the action bar relevant to this screen if the drawer is not showing.
            // Otherwise, let the drawer decide what to show in the action bar.
            if (!drawerFragment.IsDrawerOpen())
            {
                // MenuInflater.Inflate(Resource.Menu.xxx, menu);
                ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
                ActionBar.SetDisplayShowTitleEnabled(true);
                ActionBar.Title = beforeDrawerTitle;
            }

            return base.OnCreateOptionsMenu(menu);
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.Settings:
                    var intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

        #region >> NavigationDrawerFragment.ICallbacks

        /// <inheritdoc />
        public void OnNavigationDrawerItemSelected(int position)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (position)
            {
                case 0:
                    SupportFragmentManager
                        .BeginTransaction()
                        .Replace(Resource.Id.fragment_container, new CareersListFragment())
                        .Commit();
                    break;
                case 1:
                    SupportFragmentManager
                        .BeginTransaction()
                        .Replace(Resource.Id.fragment_container, new SettingsFragment())
                        .Commit();
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Configure the Navigation Drawer for use in this activity.
        /// </summary>
        private void ConfigureNavigationDrawer()
        {
            drawerFragment = SupportFragmentManager.FindFragmentById(Resource.Id.navigation_drawer) as NavigationDrawerFragment;
            System.Diagnostics.Debug.Assert(drawerFragment != null);

            beforeDrawerTitle = Title;

            drawerFragment.Setup(
                Resource.Id.navigation_drawer,
                FindViewById<DrawerLayout>(Resource.Id.drawer_layout));
        }
    }
}

