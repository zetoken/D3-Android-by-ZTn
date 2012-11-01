using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class CareersListFragment : Fragment
    {
        const int ADD_NEW_ACCOUNT = 0;

        readonly String[] accountsFromColumns = new String[] { Storage.AccountsOpenHelper.FIELD_BATTLETAG, Storage.AccountsOpenHelper.FIELD_HOST };
        readonly int[] accountsToId = new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };

        IList<IDictionary<String, Object>> accountsList = new JavaList<IDictionary<String, Object>>();

        ICursor cursor;

        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Console.WriteLine("OnActivityResult");
            switch (requestCode)
            {
                case ADD_NEW_ACCOUNT:
                    switch (resultCode)
                    {
                        case Result.Ok:
                            String battleTag = data.GetStringExtra("battleTag");
                            String host = data.GetStringExtra("host");

                            D3Context.getInstance().dbAccounts.insert(battleTag, host);

                            IListAdapter careerAdapter = new SimpleCursorAdapter(Activity, Android.Resource.Layout.SimpleListItem2, cursor, accountsFromColumns, accountsToId);
                            Activity.FindViewById<ListView>(Resource.Id.AccountsListView).Adapter = careerAdapter;

                            Toast.MakeText(Activity, "Account added", ToastLength.Short).Show();
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

        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("OnCreate");
            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            Console.WriteLine("OnCreateOptionsMenu");
            base.OnCreateOptionsMenu(menu, inflater);

            Activity.MenuInflater.Inflate(Resource.Menu.HomeActivity, menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("OnCreateView");
            View view = inflater.Inflate(Resource.Layout.Home, container, false);

            return view;
        }

        public override void OnDestroy()
        {
            Activity.StopManagingCursor(cursor);
            cursor.Close();

            base.OnDestroy();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Console.WriteLine("OnOptionsItemSelected");
            switch (item.ItemId)
            {
                case Resource.Id.AddNewAccount:
                    Intent intent = new Intent(Activity, typeof(AddNewAccountActivity));
                    StartActivityForResult(intent, ADD_NEW_ACCOUNT);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnResume()
        {
            Console.WriteLine("OnResume");
            base.OnResume();

            ListView careerListView = Activity.FindViewById<ListView>(Resource.Id.AccountsListView);
            careerListView.ItemClick += (Object sender, AdapterView.ItemClickEventArgs args) =>
            {
                Intent viewCareerIntent = new Intent(Activity, typeof(ViewCareerActivity));
                viewCareerIntent.PutExtra("battleTag", args.View.FindViewById<TextView>(Android.Resource.Id.Text1).Text);
                viewCareerIntent.PutExtra("host", args.View.FindViewById<TextView>(Android.Resource.Id.Text2).Text);
                StartActivity(viewCareerIntent);
            };

            D3Context.getInstance().dbAccounts = new AccountsDB(Activity);
            cursor = D3Context.getInstance().dbAccounts.getAccounts();
            Activity.StartManagingCursor(cursor);

            IListAdapter accountsAdapter = new SimpleCursorAdapter(Activity, Android.Resource.Layout.SimpleListItem2, cursor, accountsFromColumns, accountsToId);
            Activity.FindViewById<ListView>(Resource.Id.AccountsListView).Adapter = accountsAdapter;
        }

        private void insertIntoCareersStorage(String battleTag, String host)
        {
            D3Context.getInstance().dbAccounts.insert(battleTag, host);
        }
    }
}