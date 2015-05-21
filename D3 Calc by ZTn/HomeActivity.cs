using Android.App;
using Android.OS;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/HomeActivityLabel", MainLauncher = true)]
    public class HomeActivity : ZTnNavigationDrawerActivity
    {
        #region >> FragmentActivity

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
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

