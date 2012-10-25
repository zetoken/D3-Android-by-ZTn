using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Heroes;

namespace ZTn.Tests
{
    [Activity(Label = "D3 Calc by ZTn", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : Activity
    {
        const int ADD_NEW_PROFILE = 0;

        readonly String[] careersSimpleAdapterFrom = new String[] { "battleTag", "host" };
        readonly int[] careersSimpleAdapterTo = new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };

        IList<IDictionary<String, Object>> careersList = new JavaList<IDictionary<String, Object>>();

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (requestCode)
            {
                case ADD_NEW_PROFILE:
                    String message = String.Format("Activity \"Add New Profile\" result: {0}", resultCode);
                    Toast.MakeText(this, message, ToastLength.Short).Show();

                    switch (resultCode)
                    {
                        case Result.Ok:
                            JavaDictionary<String, Object> careerItem = new JavaDictionary<String, Object>();
                            careerItem.Add("battleTag", data.GetStringExtra("battleTag"));
                            careerItem.Add("host", data.GetStringExtra("host"));
                            careersList.Add(careerItem);

                            IListAdapter careerAdapter = new SimpleAdapter(this, careersList, Android.Resource.Layout.SimpleListItem2, careersSimpleAdapterFrom, careersSimpleAdapterTo);

                            FindViewById<ListView>(Resource.Id.CareersListView).Adapter = careerAdapter;
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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Home);

            ListView careerListView = FindViewById<ListView>(Resource.Id.CareersListView);
            careerListView.ItemClick += (Object sender, AdapterView.ItemClickEventArgs args) =>
            {
                Intent viewCareerIntent = new Intent(this, typeof(ViewCareerActivity));
                viewCareerIntent.PutExtra("battleTag", args.View.FindViewById<TextView>(Android.Resource.Id.Text1).Text);
                viewCareerIntent.PutExtra("host", args.View.FindViewById<TextView>(Android.Resource.Id.Text2).Text);
                StartActivity(viewCareerIntent);
            };

            careersList.Add(new JavaDictionary<String, Object>() { { "battleTag", "Tok#2360" }, { "host", "eu.battle.net" } });
            IListAdapter careersAdapter = new SimpleAdapter(this, careersList, Android.Resource.Layout.SimpleListItem2, careersSimpleAdapterFrom, careersSimpleAdapterTo);
            FindViewById<ListView>(Resource.Id.CareersListView).Adapter = careersAdapter;
        }

        /// <summary>
        /// Called when creating action bar
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainActivity, menu);

            return true;
        }

        /// <summary>
        /// Called when an item is selected in the action bar
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.AddNewProfile:
                    this.StartActivityForResult(typeof(AddNewProfileActivity), ADD_NEW_PROFILE);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}

