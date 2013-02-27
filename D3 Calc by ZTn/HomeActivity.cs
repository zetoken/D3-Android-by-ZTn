using System;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using ZTn.BNet.D3;
using ZTn.BNet.D3.DataProviders;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

[assembly: Application(Icon = "@drawable/icon", Label = "D3 Calc by ZTn", Theme = "@android:style/Theme.Holo")]
namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/HomeActivityLabel", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : FragmentActivity
    {
        public static readonly String SETTINGS_FILENAME = "settings";
        public static readonly String SETTINGS_ONLINEMODE = "onlineMode";

        public static ISharedPreferences preferences;

        private static Fragment careersListFragment;

        public override void OnBackPressed()
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnBackPressed();

            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FragmentContainer);

            // Load preferences
            preferences = GetSharedPreferences(SETTINGS_FILENAME, FileCreationMode.Private);
            // Default offline mode
            D3Context.getInstance().onlineMode = (preferences.GetBoolean(SETTINGS_ONLINEMODE, false) ? OnlineMode.Online : OnlineMode.Offline);

            // Always start D3Api with cache available
            DataProviders.CacheableDataProvider dataProvider = new DataProviders.CacheableDataProvider(this, new ZTn.BNet.D3.DataProviders.HttpRequestDataProvider());
            dataProvider.onlineMode = D3Context.getInstance().onlineMode;
            D3Api.dataProvider = dataProvider;

            // Set english locale by default
            D3Api.locale = "en";

            // Update fragments
            if (savedInstanceState == null)
            {
                careersListFragment = new CareersListFragment();
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.fragment_container, careersListFragment)
                    .Commit();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            this.MenuInflater.Inflate(Resource.Menu.Settings, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.Settings:
                    Intent intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}

