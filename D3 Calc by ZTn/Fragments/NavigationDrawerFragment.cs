using System;
using System.Reflection;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.NavigationDrawer;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class NavigationDrawerFragment : Fragment
    {
        /// <summary>
        /// Remember the position of the selected item.
        /// </summary>
        const string StateSelectedPosition = "SelectedNavigationDrawerPosition";

        int currentSelectedPosition;

        /// <summary>
        /// Helper component that ties the action bar to the navigation drawer.
        /// </summary>
        private NavigationDrawerToggle drawerToggle;

        DrawerLayout drawerLayout;

        ListView drawerListView;

        View fragmentContainerView;

        public EventHandler<NavigationDrawerItemSelectedEventArgs> ItemSelected { get; set; }

        private void SelectItem(int position)
        {
            currentSelectedPosition = position;
            if (drawerListView != null)
            {
                drawerListView.SetItemChecked(position, true);
            }
            if (drawerLayout != null)
            {
                drawerLayout.CloseDrawer(fragmentContainerView);
            }
            if (ItemSelected != null)
            {
                ItemSelected(this, new NavigationDrawerItemSelectedEventArgs { Position = position });
            }
        }

        #region >> Fragment

        /// <inheritdoc />
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            HasOptionsMenu = true;
        }

        /// <inheritdoc />
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            drawerToggle.OnConfigurationChanged(newConfig);
        }

        /// <inheritdoc />
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null)
            {
                currentSelectedPosition = savedInstanceState.GetInt(StateSelectedPosition);
            }
        }

        /// <inheritdoc />
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            drawerListView = (ListView)inflater.Inflate(Resource.Layout.NavigationDrawer, container, false);

            drawerListView.ItemClick += (sender, e) => SelectItem(e.Position);

            var listItems = new[]
            {
                Resources.GetString(Resource.String.BattleTags),
                Resources.GetString(Resource.String.Settings)
            };

            drawerListView.Adapter = new ArrayAdapter<string>(
                Activity.ActionBar.ThemedContext,
                Android.Resource.Layout.SimpleListItemActivated1,
                Android.Resource.Id.Text1,
                listItems);

            drawerListView.SetItemChecked(currentSelectedPosition, true);

            return drawerListView;
        }

        /// <inheritdoc />
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // This call allows the drawer to be automatically opened/closed when touching "Up" action bar button.
            if (drawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <inheritdoc />
        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt(StateSelectedPosition, currentSelectedPosition);
        }

        #endregion

        /// <summary>
        /// Inform if Navigation Drawer is opened (shown).
        /// </summary>
        /// <returns></returns>
        public bool IsDrawerOpen()
        {
            return drawerLayout != null && drawerLayout.IsDrawerOpen(fragmentContainerView);
        }

        /// <summary>
        /// Users of this fragment must call this method to set up the navigation drawer interactions.
        /// </summary>
        /// <param name="fragmentId">The <c>android:id</c> of this fragment in its activity's layout.</param>
        /// <param name="drawerLayout">The <see cref="DrawerLayout"/> containing this fragment's UI.</param>
        public void Setup(int fragmentId, DrawerLayout drawerLayout)
        {
            this.drawerLayout = drawerLayout;

            fragmentContainerView = Activity.FindViewById(fragmentId);

            drawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow, GravityCompat.Start);

            drawerToggle = new NavigationDrawerToggle(Activity,
                drawerLayout,
                0,
                0);

            drawerToggle.Closed += (sender, args) => Activity.InvalidateOptionsMenu();
            drawerToggle.Opened += (sender, args) => Activity.InvalidateOptionsMenu();

            drawerToggle.SyncState();

            drawerLayout.SetDrawerListener(drawerToggle);

            // Select either the default item (0) or the last selected item.
            SelectItem(currentSelectedPosition);
        }
    }
}