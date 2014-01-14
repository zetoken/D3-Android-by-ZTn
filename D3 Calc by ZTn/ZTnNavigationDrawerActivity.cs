using System.Reflection;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Java.IO;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator
{
    public class ZTnNavigationDrawerActivity : FragmentActivity, NavigationDrawerFragment.ICallbacks
    {
        protected NavigationDrawerFragment DrawerFragment;

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

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetIcon(Resource.Drawable.ic_launcher);

        }

        /// <inheritdoc/>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            // Only show items in the action bar relevant to this screen if the drawer is not showing.
            // Otherwise, let the drawer decide what to show in the action bar.
            if (!DrawerFragment.IsDrawerOpen())
            {
            }

            return base.OnCreateOptionsMenu(menu);
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            //switch (item.ItemId)
            //{
            //    case Android.Resource.Id.Home:
            //        if (DrawerFragment.IsDrawerOpen())
            //        {

            //        }
            //        return true;
            //}

            return base.OnOptionsItemSelected(item);
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
        protected void ConfigureNavigationDrawer()
        {
            DrawerFragment = SupportFragmentManager.FindFragmentById(Resource.Id.navigation_drawer) as NavigationDrawerFragment;
            System.Diagnostics.Debug.Assert(DrawerFragment != null);

            DrawerFragment.Setup(
                Resource.Id.navigation_drawer,
                FindViewById<DrawerLayout>(Resource.Id.drawer_layout));
        }
    }
}