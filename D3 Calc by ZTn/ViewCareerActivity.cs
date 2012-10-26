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
        readonly String[] heroesSimpleAdapterFrom = new String[] { "name", "details" };
        readonly int[] heroesSimpleAdapterTo = new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };

        IList<IDictionary<String, Object>> heroesList = new JavaList<IDictionary<String, Object>>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ViewCareer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById<TextView>(Resource.Id.BattleTagTextView).Text = Intent.GetStringExtra("battleTag");

            fetchCareer(Intent.GetStringExtra("battleTag"), Intent.GetStringExtra("host"));
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void fetchCareer(String battleTag, String host)
        {
            D3Api.host = host;
            Career career;

            try
            {
                career = Career.getCareerFromBattleTag(new BattleTag(battleTag));
            }
            catch (Exception exception)
            {
                Toast.MakeText(this, "An error occured when retrieving the career" + System.Environment.NewLine + exception, ToastLength.Long).Show();
                return;
            }

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
                    heroItem.Add("name", hero.name);
                    heroItem.Add("details", String.Format("{0} Level {1} ({2})", hero.heroClass, hero.level, hero.paragonLevel));
                    heroesList.Add(heroItem);
                }

                IListAdapter heroesAdapter = new SimpleAdapter(this, heroesList, Android.Resource.Layout.SimpleListItem2, heroesSimpleAdapterFrom, heroesSimpleAdapterTo);
                FindViewById<ListView>(Resource.Id.HeroesListView).Adapter = heroesAdapter;
            }
        }

    }
}