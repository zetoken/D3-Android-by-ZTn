using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/AddNewAccountActivityLabel")]
    public class AddNewAccountActivity : ZTnFragmentActivity
    {
        Spinner serverSpinner;
        EditText battleTagEditText;

        #region >> ZTnFragmentActivity

        /// <inheritdoc/>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AddNewAccount);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            var hosts = Assets.Open("hosts.json").CreateFromJsonStream<List<Host>>();

            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, hosts.Select(h => h.Url).ToArray());
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            serverSpinner = FindViewById<Spinner>(Resource.Id.hostSpinner);
            serverSpinner.Adapter = adapter;

            battleTagEditText = FindViewById<EditText>(Resource.Id.battleTagEditText);
        }

        /// <inheritdoc/>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.AddNewAccountActivity, menu);

            return true;
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.SubmitNewAccount:
                    OnSubmit();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

        private void OnSubmit()
        {
            try
            {
                var battleTag = new BattleTag(battleTagEditText.Text);

                var resultIntent = new Intent();
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