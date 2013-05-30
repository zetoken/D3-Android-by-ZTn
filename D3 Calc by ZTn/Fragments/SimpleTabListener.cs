using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class SimpleTabListener<T> : Java.Lang.Object, ActionBar.ITabListener
        where T : Fragment
    {
        Fragment fragment;
        Context context;

        #region >> Constructors

        public SimpleTabListener(Context context)
        {
            this.context = context;
            fragment = null;
        }

        #endregion

        #region >> ActionBar.ITabListener

        /// <inheritdoc/>
        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            fragmentTransaction.Detach(fragment);
            fragmentTransaction.Attach(fragment);
        }

        /// <inheritdoc/>
        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

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

        /// <inheritdoc/>
        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            if (fragment != null)
            {
                fragmentTransaction.Detach(fragment);
            }
        }

        #endregion
    }
}