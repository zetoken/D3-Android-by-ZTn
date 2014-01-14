using System;
using System.Reflection;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Views;
using Android.Widget;

using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;

using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class CareersListFragment : Fragment
    {
        const int AddNewAccount = 0;

        readonly String[] accountsFromColumns = { AccountsOpenHelper.FIELD_BATTLETAG, AccountsOpenHelper.FIELD_HOST };
        readonly int[] accountsToId = { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };

        ICursor cursor;

        #region >> Fragment

        /// <inheritdoc/>
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (requestCode)
            {
                case AddNewAccount:
                    switch (resultCode)
                    {
                        case -1:
                            var battleTag = data.GetStringExtra("battleTag");
                            var host = data.GetStringExtra("host");

                            D3Context.Instance.DBAccounts.Insert(battleTag, host);

                            IListAdapter careerAdapter = new SimpleCursorAdapter(Activity, Android.Resource.Layout.SimpleListItem2, cursor, accountsFromColumns, accountsToId);
                            Activity.FindViewById<ListView>(Resource.Id.AccountsListView).Adapter = careerAdapter;

                            Toast.MakeText(Activity, "Account added", ToastLength.Short).Show();
                            break;
                    }
                    break;
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            RetainInstance = true;

            SetHasOptionsMenu(true);

            Activity.Title = Resources.GetString(Resource.String.HomeActivityLabel);
        }

        /// <inheritdoc/>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            Activity.MenuInflater.Inflate(Resource.Menu.HomeActivity, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var view = inflater.Inflate(Resource.Layout.Home, container, false);

            var careerListView = view.FindViewById<ListView>(Resource.Id.AccountsListView);
            careerListView.ItemClick += (sender, args) =>
            {
                var viewCareerIntent = new Intent(Activity, typeof(ViewCareerActivity));
                D3Context.Instance.BattleTag = args.View.FindViewById<TextView>(Android.Resource.Id.Text1).Text;
                D3Context.Instance.Host = args.View.FindViewById<TextView>(Android.Resource.Id.Text2).Text;
                StartActivity(viewCareerIntent);
            };

            D3Context.Instance.DBAccounts = new AccountsDB(Activity);
            cursor = D3Context.Instance.DBAccounts.GetAccounts();
            Activity.StartManagingCursor(cursor);

            IListAdapter accountsAdapter = new SimpleCursorAdapter(Activity, Android.Resource.Layout.SimpleListItem2, cursor, accountsFromColumns, accountsToId);
            view.FindViewById<ListView>(Resource.Id.AccountsListView).Adapter = accountsAdapter;

            return view;
        }

        /// <inheritdoc/>
        public override void OnDestroyView()
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            Activity.StopManagingCursor(cursor);
            cursor.Close();

            base.OnDestroyView();
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.AddNewAccount:
                    var intent = new Intent(Activity, typeof(AddNewAccountActivity));
                    StartActivityForResult(intent, AddNewAccount);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion
    }
}