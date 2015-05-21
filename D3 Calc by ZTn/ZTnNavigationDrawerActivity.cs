using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using ZTnDroid.D3Calculator.Fragments;

namespace ZTnDroid.D3Calculator
{
    public class ZTnNavigationDrawerActivity : FragmentActivity
    {
        protected NavigationDrawerFragment DrawerFragment;

        #region >> FragmentActivity

        /// <inheritdoc />
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        #endregion

        /// <inheritdoc />
        private void OnNavigationDrawerItemSelected(int position)
        {
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

        /// <summary>
        /// Configure the Navigation Drawer for use in this activity.
        /// </summary>
        protected void ConfigureNavigationDrawer()
        {
            DrawerFragment = (NavigationDrawerFragment)SupportFragmentManager.FindFragmentById(Resource.Id.navigation_drawer);

            DrawerFragment.ItemSelected += (sender, args) => OnNavigationDrawerItemSelected(args.Position);

            DrawerFragment.Setup(
                Resource.Id.navigation_drawer,
                FindViewById<DrawerLayout>(Resource.Id.drawer_layout));
        }
    }
}