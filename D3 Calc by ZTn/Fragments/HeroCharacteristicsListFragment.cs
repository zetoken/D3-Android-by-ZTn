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
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroCharacteristicsListFragment : Fragment
    {
        String battleTag;
        String host;
        HeroSummary heroSummary;

        Hero hero;

        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroCharacteristicsListFragment: OnCreate");
            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            Activity.MenuInflater.Inflate(Resource.Menu.ViewHeroActivity, menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("HeroCharacteristicsListFragment: OnCreateView");
            View view = inflater.Inflate(Resource.Layout.ViewHero, container, false);

            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;
            heroSummary = D3Context.getInstance().heroSummary;

            Activity.Title = heroSummary.name;
            Activity.ActionBar.Subtitle = battleTag;

            deferredFetchAndUpdateHero(false);

            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.RefreshContent:
                    deferredFetchAndUpdateHero(true);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void deferredFetchAndUpdateHero(Boolean online)
        {
            ProgressDialog progressDialog = null;

            if (online)
                progressDialog = ProgressDialog.Show(Activity, "Loading Hero", "Please wait while retrieving data", true);

            new Thread(new ThreadStart(() =>
            {
                Hero hero = null;

                try
                {
                    hero = fetchHero(online);
                    Activity.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        updateHeroView();
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(Activity, "Hero not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(Activity, "An error occured when retrieving the hero", ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            })).Start();
        }

        private Hero fetchHero(Boolean online)
        {
            D3Api.host = host;
            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.online = online;

            try
            {
                hero = Hero.getHeroFromHeroId(new BattleTag(battleTag), heroSummary.id);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                hero = null;
                throw exception;
            }
            finally
            {
                dataProvider.online = false;
            }

            return hero;
        }

        private void updateHeroView()
        {
            if (hero != null)
            {
                ListView heroStatsListView = Activity.FindViewById<ListView>(Resource.Id.heroStatsListView);
                List<IListItem> characteristicsAttr = new List<IListItem>()
                {
                    new SectionHeaderListItem("PROGRESS"),
                    new AttributeListItem("level", hero.level),
                    new AttributeListItem("paragon", hero.paragonLevel),
                    new SectionHeaderListItem("LIFETIME KILLS"),
                    new AttributeListItem("elites", hero.kills.elites),
                    new SectionHeaderListItem("CHARACTERISTICS"),
                    new AttributeListItem("dexterity", hero.stats.dexterity),
                    new AttributeListItem("intelligence", hero.stats.intelligence),
                    new AttributeListItem("strength", hero.stats.strength),
                    new AttributeListItem("vitality", hero.stats.vitality),
                    new SectionHeaderListItem("DAMAGES"),
                    new AttributeListItem("damage", hero.stats.damage),
                    new AttributePercentListItem("critic chance", hero.stats.critChance),
                    new AttributePercentListItem("critic damage", hero.stats.critDamage),
                    new SectionHeaderListItem("LIFE"),
                    new AttributeListItem("life", hero.stats.life),
                    new AttributeListItem("life on hit", hero.stats.lifeOnHit),
                    new AttributePercentListItem("life steal", hero.stats.lifeSteal),
                    new AttributeListItem("life per kill", hero.stats.lifePerKill),
                    new SectionHeaderListItem("DEFENSE"),
                    new AttributeListItem("armor", hero.stats.armor),
                    new AttributeListItem("arcane resist", hero.stats.arcaneResist),
                    new AttributeListItem("cold resist", hero.stats.coldResist),
                    new AttributeListItem("fire resist", hero.stats.fireResist),
                    new AttributeListItem("lightning resist", hero.stats.lightningResist),
                    new AttributeListItem("physical resist", hero.stats.physicalResist),
                    new AttributeListItem("poison resist", hero.stats.poisonResist),
                    new SectionHeaderListItem("BONUS"),
                    new AttributePercentListItem("gold find", hero.stats.goldFind),
                    new AttributePercentListItem("magic find", hero.stats.magicFind),
                };
                heroStatsListView.Adapter = new SectionedListAdapter(Activity, characteristicsAttr.ToArray());
            }
        }

    }
}