using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class SimpleTabListener<T> : Java.Lang.Object, ActionBar.ITabListener
        where T: Fragment, new()
    {
        Fragment fragment;

        public SimpleTabListener()
        {
            this.fragment = new T();
        }

         public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            // Nothing to do
        }

        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            fragmentTransaction.Add(Resource.Id.fragment_container, fragment, typeof(T).Name);
        }

        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            fragmentTransaction.Remove(fragment);
        }
    }
}