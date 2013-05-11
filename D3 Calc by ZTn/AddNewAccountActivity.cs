using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/AddNewAccountActivityLabel")]
    public class AddNewAccountActivity : ZTnFragmentActivity
    {
        Spinner serverSpinner;
        EditText battleTagEditText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AddNewAccount);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            List<Host> hosts = JsonHelpers.getDataFromJSonStream<Host>(Assets.Open("hosts.json"));

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, hosts.Select(h => h.url).ToArray());
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            serverSpinner = FindViewById<Spinner>(Resource.Id.hostSpinner);
            serverSpinner.Adapter = adapter;

            battleTagEditText = FindViewById<EditText>(Resource.Id.battleTagEditText);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.AddNewAccountActivity, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.SubmitNewAccount:
                    onSubmit();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void onSubmit()
        {
            BattleTag battleTag = null;
            try
            {
                battleTag = new BattleTag(battleTagEditText.Text);

                Intent resultIntent = new Intent();
                resultIntent.PutExtra("battleTag", battleTag.ToString());
                resultIntent.PutExtra("host", serverSpinner.SelectedItem.ToString());

                SetResult(Result.Ok, resultIntent);
                Finish();
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Battle Tag format is incorrect", ToastLength.Long).Show();
            }
        }
    }
}