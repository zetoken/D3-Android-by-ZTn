using System;
using Android.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;

namespace ZTnDroid.D3Calculator.NavigationDrawer
{
    /// <summary>
    /// ActionBarDrawerToggle implementation.
    /// </summary>
    public class NavigationDrawerToggle : ActionBarDrawerToggle
    {
        public EventHandler<NavigationDrawerChangedEventArgs> Closed { get; set; }
        public EventHandler<NavigationDrawerChangedEventArgs> Opened { get; set; }

        public NavigationDrawerToggle(Activity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
        }

        #region >> ActionBarDrawerToggle

        /// <inheritdoc />
        public override void OnDrawerClosed(View drawerView)
        {
            if (Closed != null)
            {
                Closed(this, new NavigationDrawerChangedEventArgs { DrawerView = drawerView });
            }

            base.OnDrawerClosed(drawerView);
        }

        /// <inheritdoc />
        public override void OnDrawerOpened(View drawerView)
        {
            if (Opened != null)
            {
                Opened(this, new NavigationDrawerChangedEventArgs { DrawerView = drawerView });
            }

            base.OnDrawerOpened(drawerView);
        }

        #endregion
    }
}