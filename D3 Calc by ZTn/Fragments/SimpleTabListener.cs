using Android.App;
using Android.Content;
using System.Reflection;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class SimpleTabListener<T> : Java.Lang.Object, ActionBar.ITabListener
        where T : Fragment
    {
        Fragment fragment;
        readonly Context context;

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
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            fragmentTransaction.Detach(fragment);
            fragmentTransaction.Attach(fragment);
        }

        /// <inheritdoc/>
        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

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
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            if (fragment != null)
            {
                fragmentTransaction.Detach(fragment);
            }
        }

        #endregion
    }
}