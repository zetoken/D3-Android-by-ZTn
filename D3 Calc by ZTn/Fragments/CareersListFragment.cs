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

        ICursor cursor;

        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Console.WriteLine("CareersListFragment: OnActivityResult");
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
            Console.WriteLine("CareersListFragment: OnCreate");
            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            Activity.MenuInflater.Inflate(Resource.Menu.HomeActivity, menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("CareersListFragment: OnCreateView");
            View view = inflater.Inflate(Resource.Layout.Home, container, false);

            ListView careerListView = view.FindViewById<ListView>(Resource.Id.AccountsListView);
            careerListView.ItemClick += (Object sender, AdapterView.ItemClickEventArgs args) =>
            {
                Intent viewCareerIntent = new Intent(Activity, typeof(ViewCareerActivity));
                D3Context.getInstance().battleTag = args.View.FindViewById<TextView>(Android.Resource.Id.Text1).Text;
                D3Context.getInstance().host = args.View.FindViewById<TextView>(Android.Resource.Id.Text2).Text;
                StartActivity(viewCareerIntent);
            };

            D3Context.getInstance().dbAccounts = new AccountsDB(Activity);
            cursor = D3Context.getInstance().dbAccounts.getAccounts();
            Activity.StartManagingCursor(cursor);

            IListAdapter accountsAdapter = new SimpleCursorAdapter(Activity, Android.Resource.Layout.SimpleListItem2, cursor, accountsFromColumns, accountsToId);
            view.FindViewById<ListView>(Resource.Id.AccountsListView).Adapter = accountsAdapter;

            return view;
        }

        public override void OnDestroyView()
        {
            Activity.StopManagingCursor(cursor);
            cursor.Close();

            base.OnDestroyView();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Console.WriteLine("CareersListFragment: OnOptionsItemSelected");
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

        private void insertIntoCareersStorage(String battleTag, String host)
        {
            D3Context.getInstance().dbAccounts.insert(battleTag, host);
        }
    }
}