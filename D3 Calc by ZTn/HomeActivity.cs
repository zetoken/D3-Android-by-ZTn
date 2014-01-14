using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using System.Reflection;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/HomeActivityLabel", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : ZTnNavigationDrawerActivity
    {
        #region >> FragmentActivity

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NavigationDrawerFragmentContainer);

            if (savedInstanceState == null)
            {
                ConfigureNavigationDrawer();
            }
        }

        #endregion
    }
}

