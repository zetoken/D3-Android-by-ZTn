using System.Reflection;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Preferences;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ZTnDroid.D3Calculator.Helpers;
using Fragment = Android.Support.V4.App.Fragment;
using String = System.String;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class NavigationDrawerFragment : Fragment
    {
        /// <summary>
        /// Per the design guidelines, you should show the drawer on launch until the user manually expands it. This shared preference tracks this.
        /// </summary>
        const String PrefUserLearnedDrawer = "NavigationDrawerLearned";

        /// <summary>
        /// Remember the position of the selected item.
        /// </summary>
        const String StateSelectedPosition = "SelectedNavigationDrawerPosition";

        bool userLearnedDrawer;

        int currentSelectedPosition;

        bool fromSavedInstanceState;

        /// <summary>
        /// Helper component that ties the action bar to the navigation drawer.
        /// </summary>
        private ActionBarDrawerToggle drawerToggle;

        DrawerLayout drawerLayout;

        ListView drawerListView;

        View fragmentContainerView;

        /// <summary>
        /// A pointer to the current callbacks instance (the <see cref="Activity"/>).
        /// </summary>
        private ICallbacks drawerCallbacks;

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
            if (drawerCallbacks != null)
            {
                drawerCallbacks.OnNavigationDrawerItemSelected(position);
            }
        }

        #region >> Fragment

        /// <inheritdoc />
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            SetHasOptionsMenu(true);
        }

        /// <inheritdoc />
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);

            drawerCallbacks = activity as ICallbacks;
            System.Diagnostics.Debug.Assert(drawerCallbacks != null);
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

            var preferences = PreferenceManager.GetDefaultSharedPreferences(Activity);
            userLearnedDrawer = preferences.GetBoolean(PrefUserLearnedDrawer, false);

            if (savedInstanceState != null)
            {
                currentSelectedPosition = savedInstanceState.GetInt(StateSelectedPosition);
                fromSavedInstanceState = true;
            }

            // Select either the default item (0) or the last selected item.
            SelectItem(currentSelectedPosition);
        }

        /// <inheritdoc />
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            if (IsDrawerOpen())
            {
                // TODO ? inflater.Inflate(Resource.Menu.SomeMenuDefinition, menu);
                // TODO ? Activity.ActionBar.Title = Resources.GetString(Resource.String.ApplicationName);
            }

            base.OnCreateOptionsMenu(menu, inflater);
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
        public override void OnDetach()
        {
            base.OnDetach();

            drawerCallbacks = null;
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

            var actionBar = Activity.ActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetHomeButtonEnabled(true);

            drawerToggle = new ActionBarToggle(
                this,
                Activity,
                drawerLayout,
                Resource.Drawable.ic_navigation_drawer,
                0,
                0);

            if (!userLearnedDrawer && !fromSavedInstanceState)
            {
                drawerLayout.OpenDrawer(fragmentContainerView);
            }

            // Defer code dependent on restoration of previous instance state.
            drawerLayout.Post(new Runnable(() => drawerToggle.SyncState()));

            drawerLayout.SetDrawerListener(drawerToggle);
        }

        /// <summary>
        /// Callbacks interface that all activities using this fragment must implement.
        /// </summary>
        public interface ICallbacks
        {
            /// <summary>
            /// Called when an item in the navigation drawer is selected.
            /// </summary>
            /// <param name="position"></param>
            void OnNavigationDrawerItemSelected(int position);
        }

        /// <summary>
        /// ActionBarDrawerToggle implementation.
        /// </summary>
        private class ActionBarToggle : ActionBarDrawerToggle
        {
            readonly NavigationDrawerFragment navigationDrawerFragment;
            readonly Activity activity;

            public ActionBarToggle(NavigationDrawerFragment navigationDrawerFragment, Activity activity, DrawerLayout drawerLayout, int drawerImageRes, int openDrawerContentDescRes, int closeDrawerContentDescRes)
                : base(activity, drawerLayout, drawerImageRes, openDrawerContentDescRes, closeDrawerContentDescRes)
            {
                this.activity = activity;
                this.navigationDrawerFragment = navigationDrawerFragment;
            }

            #region >> ActionBarDrawerToggle

            /// <inheritdoc />
            public override void OnDrawerClosed(View drawerView)
            {
                ZTnTrace.Trace(MethodBase.GetCurrentMethod());

                base.OnDrawerClosed(drawerView);

                if (!navigationDrawerFragment.IsAdded)
                {
                    return;
                }

                // Force call OnPrepareOptionsMenu()
                activity.InvalidateOptionsMenu();
            }

            /// <inheritdoc />
            public override void OnDrawerOpened(View drawerView)
            {
                ZTnTrace.Trace(MethodBase.GetCurrentMethod());

                base.OnDrawerOpened(drawerView);

                if (!navigationDrawerFragment.IsAdded)
                {
                    return;
                }

                if (!navigationDrawerFragment.userLearnedDrawer)
                {
                    // The user manually opened the drawer; store this flag to prevent auto-showing the navigation drawer automatically in the future.
                    navigationDrawerFragment.userLearnedDrawer = true;
                    PreferenceManager.GetDefaultSharedPreferences(activity)
                        .Edit()
                        .PutBoolean(PrefUserLearnedDrawer, true)
                        .Apply();
                }

                // Force call OnPrepareOptionsMenu()
                activity.InvalidateOptionsMenu();
            }

            #endregion
        }
    }
}