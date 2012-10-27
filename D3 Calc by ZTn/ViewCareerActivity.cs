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
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Adapters;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "View Career", Theme = "@android:style/Theme.Holo")]
    public class ViewCareerActivity : Activity
    {
        String battleTag;
        String host;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ViewCareer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            battleTag = Intent.GetStringExtra("battleTag");
            host = Intent.GetStringExtra("host");

            Title = battleTag;
        }

        protected override void OnResume()
        {
            base.OnResume();

            updateCareerView(fetchCareer(battleTag, host));
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

                case Resource.Id.RefreshContent:
                    DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
                    dataProvider.online = true;
                    updateCareerView(fetchCareer(battleTag, host));
                    dataProvider.online = false;
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private Career fetchCareer(String battleTag, String host)
        {
            D3Api.host = host;
            Career career;

            try
            {
                career = Career.getCareerFromBattleTag(new BattleTag(battleTag));
            }
            catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
            {
                Toast.MakeText(this, "Career not in cache: please use refresh action", ToastLength.Long).Show();
                return null;
            }
            catch (Exception exception)
            {
                Toast.MakeText(this, "An error occured when retrieving the career", ToastLength.Long).Show();
                Console.WriteLine(@"/!\ An error occured when retrieving the career" + System.Environment.NewLine + exception, ToastLength.Long);
                return null;
            }

            return career;
        }

        private void updateCareerView(Career career)
        {
            if (career != null)
            {
                FindViewById<TextView>(Resource.Id.CareerKilledElites).Text = career.kills.elites.ToString();
                FindViewById<TextView>(Resource.Id.CareerKilledMonsters).Text = career.kills.monsters.ToString();
                FindViewById<TextView>(Resource.Id.CareerKilledHardcore).Text = career.kills.hardcoreMonsters.ToString();

                if (career.heroes != null)
                {
                    ListView listView = FindViewById<ListView>(Resource.Id.HeroesListView);
                    listView.Adapter = new HeroSummariesListAdapter(this, career.heroes);
                    listView.ItemClick += (Object sender, Android.Widget.AdapterView.ItemClickEventArgs args) =>
                    {
                        HeroSummary hero = ((HeroSummariesListAdapter)listView.Adapter).getHeroSummaryAt(args.Position);
                        Toast.MakeText(this, String.Format("Hero {0} was clicked", hero.name), ToastLength.Long).Show();
                    };
                }
            }
        }
    }
}