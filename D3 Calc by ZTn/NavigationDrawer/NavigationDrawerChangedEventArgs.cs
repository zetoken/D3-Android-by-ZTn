using System;
using Android.Views;

namespace ZTnDroid.D3Calculator.NavigationDrawer
{
    public class NavigationDrawerChangedEventArgs : EventArgs
    {
        public View DrawerView { get; set; }
    }
}