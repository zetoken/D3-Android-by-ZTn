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

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "New D3 Profile", Theme = "@android:style/Theme.Holo", Icon = "@android:drawable/ic_menu_add")]
    public class AddNewAccountActivity : Activity
    {
        String[] items = new String[] { "eu.battle.net", "us.battle.net", "kr.battle.net", "tw.battle.net" };

        Spinner serverSpinner;
        EditText battleTagEditText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AddNewAccount);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, items);
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

                case Android.Resource.Id.Home:
                    Finish();
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