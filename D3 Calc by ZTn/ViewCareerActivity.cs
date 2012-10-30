using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "View Career", Theme = "@android:style/Theme.Holo", Icon = "@drawable/icon")]
    public class ViewCareerActivity : Activity
    {
        String battleTag;
        String host;

        Career career;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ViewCareer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            battleTag = Intent.GetStringExtra("battleTag");
            host = Intent.GetStringExtra("host");

            D3Context.getInstance().battleTag = battleTag;
            D3Context.getInstance().host = host;

            ListView listView = FindViewById<ListView>(Resource.Id.HeroesListView);
            listView.ItemClick += (Object sender, Android.Widget.AdapterView.ItemClickEventArgs args) =>
            {
                HeroSummary heroSummary = ((HeroSummariesListAdapter)listView.Adapter).getHeroSummaryAt(args.Position);
                D3Context.getInstance().heroSummary = heroSummary;

                Intent viewHeroIntent = new Intent(this, typeof(ViewHeroActivity));

                StartActivity(viewHeroIntent);
            };

            Title = battleTag;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ViewCareerActivity, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.DeleteContent:
                    deleteCareer();
                    return true;

                case Resource.Id.RefreshContent:
                    deferredFetchAndUpdateCareer(true);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            deferredFetchAndUpdateCareer(false);
        }

        private void deferredFetchAndUpdateCareer(Boolean online)
        {
            ProgressDialog progressDialog = null;

            if (online)
                progressDialog = ProgressDialog.Show(this, "Loading Career", "Please wait while retrieving data", true);

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    fetchCareer(online);
                    this.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        updateCareerView();
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    this.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, "Career not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    this.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, "An error occured when retrieving the career", ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            })).Start();
        }

        private void deleteCareer()
        {
            Toast.MakeText(this, "Career will be removed... when implemented", ToastLength.Short);
            //...
        }

        private void fetchCareer(Boolean online)
        {
            D3Api.host = host;
            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.online = online;

            try
            {
                career = Career.getCareerFromBattleTag(new BattleTag(battleTag));
            }
            catch (Exception exception)
            {
                career = null;
                throw exception;
            }
            finally
            {
                dataProvider.online = false;
            }
        }

        private void updateCareerView()
        {
            if (career != null)
            {
                ListView killsListView = FindViewById<ListView>(Resource.Id.killsLifetimeListView);
                List<AttributeDescriptor> attributes = new List<AttributeDescriptor>()
                {
                    new AttributeDescriptor("elites", career.kills.elites.ToString()),
                    new AttributeDescriptor("monsters", career.kills.monsters.ToString()),
                    new AttributeDescriptor("hardcore", career.kills.hardcoreMonsters.ToString())
                };
                killsListView.Adapter = new AttributesListAdapter(this, attributes.ToArray());

                if (career.heroes != null)
                {
                    ListView listView = FindViewById<ListView>(Resource.Id.HeroesListView);
                    listView.Adapter = new HeroSummariesListAdapter(this, career.heroes);
                }
            }
        }
    }
}