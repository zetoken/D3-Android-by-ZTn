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

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "D3 Calc by ZTn", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : Activity
    {
        const int ADD_NEW_ACCOUNT = 0;

        readonly String[] accountsFromColumns = new String[] { Storage.AccountsOpenHelper.FIELD_BATTLETAG, Storage.AccountsOpenHelper.FIELD_HOST };
        readonly int[] accountsToId = new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };

        IList<IDictionary<String, Object>> accountsList = new JavaList<IDictionary<String, Object>>();

        Storage.AccountsDB db;
        ICursor cursor;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (requestCode)
            {
                case ADD_NEW_ACCOUNT:
                    String message = String.Format("Activity \"Add New Profile\" result: {0}", resultCode);
                    Toast.MakeText(this, message, ToastLength.Short).Show();

                    switch (resultCode)
                    {
                        case Result.Ok:
                            String battleTag = data.GetStringExtra("battleTag");
                            String host = data.GetStringExtra("host");

                            db.accountInsert(battleTag, host);

                            IListAdapter careerAdapter = new SimpleCursorAdapter(this, Android.Resource.Layout.SimpleListItem2, cursor, accountsFromColumns, accountsToId);

                            FindViewById<ListView>(Resource.Id.AccountsListView).Adapter = careerAdapter;
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnBackPressed()
        {
            Finish();
            base.OnBackPressed();
        }

        protected override void OnDestroy()
        {
            StopManagingCursor(cursor);
            cursor.Close();

            base.OnDestroy();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Home);

            ListView careerListView = FindViewById<ListView>(Resource.Id.AccountsListView);
            careerListView.ItemClick += (Object sender, AdapterView.ItemClickEventArgs args) =>
            {
                Intent viewCareerIntent = new Intent(this, typeof(ViewCareerActivity));
                viewCareerIntent.PutExtra("battleTag", args.View.FindViewById<TextView>(Android.Resource.Id.Text1).Text);
                viewCareerIntent.PutExtra("host", args.View.FindViewById<TextView>(Android.Resource.Id.Text2).Text);
                StartActivity(viewCareerIntent);
            };

            db = new Storage.AccountsDB(this);
            cursor = db.getAccounts();
            StartManagingCursor(cursor);

            IListAdapter accountsAdapter = new SimpleCursorAdapter(this, Android.Resource.Layout.SimpleListItem2, cursor, accountsFromColumns, accountsToId);

            FindViewById<ListView>(Resource.Id.AccountsListView).Adapter = accountsAdapter;

            // Always start D3Api with cache available and offline
            DataProviders.CacheableDataProvider dataProvider = new DataProviders.CacheableDataProvider(this, new ZTn.BNet.D3.DataProviders.HttpRequestDataProvider());
            dataProvider.online = false;
            D3Api.dataProvider = dataProvider;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.HomeActivity, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.AddNewAccount:
                    this.StartActivityForResult(typeof(AddNewAccountActivity), ADD_NEW_ACCOUNT);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void insertIntoCareersStorage(String battleTag, String host)
        {
            Storage.AccountsDB db = new Storage.AccountsDB(this);
            db.accountInsert(battleTag, host);
        }
    }
}

