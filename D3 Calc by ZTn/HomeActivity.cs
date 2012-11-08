using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Storage;

[assembly: Application(Icon = "@drawable/icon", Label = "D3 Calc by ZTn", Theme = "@android:style/Theme.Holo", Debuggable = false)]
namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "D3 Calc by ZTn", MainLauncher = true)]
    public class HomeActivity : Activity
    {
        public static readonly String SETTINGS_FILENAME = "settings";
        public static readonly String SETTINGS_ONLINEMODE = "onlineMode";
        public static ISharedPreferences preferences;

        public override void OnBackPressed()
        {
            Finish();
            base.OnBackPressed();
        }

        protected override void OnCreate(Bundle bundle)
        {
            Console.WriteLine("HomeActivity: OnCreate");
            base.OnCreate(bundle);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            SetContentView(Resource.Layout.FragmentContainer);

            FragmentManager
                .BeginTransaction()
                .Add(Resource.Id.fragment_container, new CareersListFragment())
                .Commit();

            // Load preferences
            preferences = GetSharedPreferences(SETTINGS_FILENAME, FileCreationMode.Private);
            // Default offline mode
            D3Context.getInstance().onlineMode = preferences.GetBoolean(SETTINGS_ONLINEMODE, false);

            // Always start D3Api with cache available
            DataProviders.CacheableDataProvider dataProvider = new DataProviders.CacheableDataProvider(this, new ZTn.BNet.D3.DataProviders.HttpRequestDataProvider());
            dataProvider.online = D3Context.getInstance().onlineMode;
            D3Api.dataProvider = dataProvider;

            // Set english locale by default
            D3Api.locale = "en";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            Console.WriteLine("HomeActivity: OnCreateOptionsMenu");

            this.MenuInflater.Inflate(Resource.Menu.Settings, menu);

            return base.OnCreateOptionsMenu(menu); ;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Console.WriteLine("CareersListFragment: OnOptionsItemSelected");
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

