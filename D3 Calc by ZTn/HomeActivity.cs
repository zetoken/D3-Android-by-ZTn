using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using System.Reflection;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/HomeActivityLabel", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : FragmentActivity
    {
        private static Fragment CareersListFragment;

        #region >> FragmentActivity

        /// <inheritdoc/>
        public override void OnBackPressed()
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnBackPressed();

            Finish();
        }

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FragmentContainer);

            // Update fragments
            if (savedInstanceState == null)
            {
                CareersListFragment = new CareersListFragment();
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.fragment_container, CareersListFragment)
                    .Commit();
            }
        }

        /// <inheritdoc/>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            MenuInflater.Inflate(Resource.Menu.Settings, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.Settings:
                    var intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion
    }
}

