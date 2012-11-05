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
        where T : Fragment
    {
        Fragment fragment;
        Context context;

        public SimpleTabListener(Context context)
        {
            this.context = context;
            fragment = null;
        }

        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            Console.WriteLine("SimpleTabListener: OnTabReselected");
            fragmentTransaction.Detach(fragment);
            fragmentTransaction.Attach(fragment);          
        }

        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            Console.WriteLine("SimpleTabListener: OnTabSelected");
            if (fragment == null)
            {
                fragment = Fragment.Instantiate(context, typeof(T).Namespace.ToLower() + "." + typeof(T).Name);
                fragmentTransaction.Add(Resource.Id.fragment_container, fragment);
            }
            else
            {
                fragmentTransaction.Attach(fragment);
            }
        }

        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            Console.WriteLine("SimpleTabListener: OnTabUnselected");
            if (fragment != null)
            {
                fragmentTransaction.Detach(fragment);
            }
        }
    }
}