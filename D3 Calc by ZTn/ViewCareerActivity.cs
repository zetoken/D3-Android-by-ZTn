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

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "View Career")]
    public class ViewCareerActivity : Activity
    {
        public static readonly String COLUMN_NAME = "name";
        public static readonly String COLUMN_DETAILS = "details";

        readonly String[] heroesSimpleAdapterFrom = new String[] { COLUMN_NAME, COLUMN_DETAILS };
        readonly int[] heroesSimpleAdapterTo = new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };

        List<IDictionary<String, Object>> heroesList = new List<IDictionary<String, Object>>();

        String battleTag;
        String host;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ViewCareer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById<TextView>(Resource.Id.BattleTagTextView).Text = Intent.GetStringExtra("battleTag");

            battleTag = Intent.GetStringExtra("battleTag");
            host = Intent.GetStringExtra("host");

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
                TextView textView = FindViewById<TextView>(Resource.Id.CareerStatsTextView);
                textView.Text = String.Format("Elites: {0}", career.kills.elites)
                    + System.Environment.NewLine
                    + String.Format("Monsters: {0}", career.kills.monsters)
                    + System.Environment.NewLine
                    + String.Format("Hardcore: {0}", career.kills.hardcoreMonsters);

                if (career.heroes != null)
                {
                    foreach (HeroSummary hero in career.heroes)
                    {
                        JavaDictionary<String, Object> heroItem = new JavaDictionary<String, Object>();
                        heroItem.Add(COLUMN_NAME, hero.name);
                        heroItem.Add(COLUMN_DETAILS, String.Format("{0} Level {1} ({2})", hero.heroClass, hero.level, hero.paragonLevel));
                        heroesList.Add(heroItem);
                    }

                    IListAdapter heroesAdapter = new SimpleAdapter(this, heroesList, Android.Resource.Layout.SimpleListItem2, heroesSimpleAdapterFrom, heroesSimpleAdapterTo);
                    ListView listView = FindViewById<ListView>(Resource.Id.HeroesListView);
                    listView.Adapter = heroesAdapter;
                    ((BaseAdapter)listView.Adapter).NotifyDataSetChanged();
                }
            }
        }
    }
}