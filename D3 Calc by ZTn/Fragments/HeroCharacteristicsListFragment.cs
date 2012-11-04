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

        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroCharacteristicsListFragment: OnCreate");
            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);

            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;
            heroSummary = D3Context.getInstance().heroSummary;
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

            deferredFetchAndUpdateHero(view, D3Context.getInstance().onlineMode);

            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.RefreshContent:
                    deferredFetchAndUpdateHero(Activity.FindViewById<View>(Resource.Id.fragment_container), true);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void deferredFetchAndUpdateHero(View view, Boolean online)
        {
            ProgressDialog progressDialog = null;

            if (online)
                progressDialog = ProgressDialog.Show(Activity, "Loading Hero", "Please wait while retrieving data", true);

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    D3Context.getInstance().hero = fetchHero(online);
                    Activity.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        updateHeroView(view);
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
            Hero hero = null;

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
                dataProvider.online = D3Context.getInstance().onlineMode;
            }

            return hero;
        }

        private void updateHeroView(View view)
        {
            Hero hero = D3Context.getInstance().hero;

            if (hero != null)
            {
                ListView heroStatsListView = view.FindViewById<ListView>(Resource.Id.heroStatsListView);
                List<IListItem> characteristicsAttr = new List<IListItem>()
                {
                    new SectionHeaderListItem(Resources.GetString(Resource.String.Progress)),
                    new AttributeListItem(Resources.GetString(Resource.String.level), hero.level),
                    new AttributeListItem(Resources.GetString(Resource.String.paragon), hero.paragonLevel),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.KillsLifetime)),
                    new AttributeListItem(Resources.GetString(Resource.String.elites), hero.kills.elites),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.attributes)),
                    new AttributeListItem(Resources.GetString(Resource.String.dexterity), hero.stats.dexterity),
                    new AttributeListItem(Resources.GetString(Resource.String.intelligence), hero.stats.intelligence),
                    new AttributeListItem(Resources.GetString(Resource.String.strength), hero.stats.strength),
                    new AttributeListItem(Resources.GetString(Resource.String.vitality), hero.stats.vitality),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.damages)),
                    new AttributeListItem(Resources.GetString(Resource.String.damage), hero.stats.damage),
                    new AttributePercentListItem(Resources.GetString(Resource.String.criticChance), hero.stats.critChance),
                    new AttributePercentListItem(Resources.GetString(Resource.String.criticDamage), hero.stats.critDamage),
                    new AttributePercentListItem(Resources.GetString(Resource.String.attackSpeed) , hero.stats.attackSpeed),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.life)),
                    new AttributeListItem(Resources.GetString(Resource.String.life), hero.stats.life),
                    new AttributeListItem(Resources.GetString(Resource.String.lifeOnHit), hero.stats.lifeOnHit),
                    new AttributePercentListItem(Resources.GetString(Resource.String.lifeSteal), hero.stats.lifeSteal),
                    new AttributeListItem(Resources.GetString(Resource.String.lifePerKill), hero.stats.lifePerKill),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.defense)),
                    new AttributeListItem(Resources.GetString(Resource.String.armor), hero.stats.armor),
                    new AttributeListItem(Resources.GetString(Resource.String.arcaneResist), hero.stats.arcaneResist),
                    new AttributeListItem(Resources.GetString(Resource.String.coldResist), hero.stats.coldResist),
                    new AttributeListItem(Resources.GetString(Resource.String.fireResist), hero.stats.fireResist),
                    new AttributeListItem(Resources.GetString(Resource.String.lightningResist), hero.stats.lightningResist),
                    new AttributeListItem(Resources.GetString(Resource.String.physicalResist), hero.stats.physicalResist),
                    new AttributeListItem(Resources.GetString(Resource.String.poisonResist), hero.stats.poisonResist),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.bonuses)),
                    new AttributePercentListItem(Resources.GetString(Resource.String.goldFind), hero.stats.goldFind),
                    new AttributePercentListItem(Resources.GetString(Resource.String.magicFind), hero.stats.magicFind),
                };
                heroStatsListView.Adapter = new SectionedListAdapter(Activity, characteristicsAttr.ToArray());
            }
        }
    }
}