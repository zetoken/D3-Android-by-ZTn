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
        where T : Fragment, new()
    {
        Fragment fragment;

        public SimpleTabListener()
        {
        }

        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            Console.WriteLine("SimpleTabListener: OnTabReselected");
            this.fragment = new T();
            fragmentTransaction.Replace(Resource.Id.fragment_container, fragment);
        }

        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            Console.WriteLine("SimpleTabListener: OnTabSelected");
            this.fragment = new T();
            fragmentTransaction.Add(Resource.Id.fragment_container, fragment);
        }

        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            Console.WriteLine("SimpleTabListener: OnTabUnselected");
            fragmentTransaction.Remove(fragment);
        }
    }
}