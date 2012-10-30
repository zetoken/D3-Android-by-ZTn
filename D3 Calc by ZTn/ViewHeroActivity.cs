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

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "View Hero", Theme = "@android:style/Theme.Holo", Icon = "@drawable/icon")]
    public class ViewHeroActivity : Activity
    {
        String battleTag;
        String host;
        HeroSummary heroSummary;

        Hero hero;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ViewHero);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;
            heroSummary = D3Context.getInstance().heroSummary;

            Title = heroSummary.name;
            ActionBar.Subtitle = battleTag;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ViewHeroActivity, menu);

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
                    deferredFetchAndUpdateHero(true);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            deferredFetchAndUpdateHero(false);
        }

        private void deferredFetchAndUpdateHero(Boolean online)
        {
            ProgressDialog progressDialog = null;

            if (online)
                progressDialog = ProgressDialog.Show(this, "Loading Hero", "Please wait while retrieving data", true);

            new Thread(new ThreadStart(() =>
            {
                Hero hero = null;

                try
                {
                    hero = fetchHero(online);
                    this.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        updateHeroView();
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    this.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, "Hero not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    this.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, "An error occured when retrieving the hero", ToastLength.Long).Show();
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
                ListView progressListView = FindViewById<ListView>(Resource.Id.progressListView);
                List<AttributeDescriptor> progressAttr = new List<AttributeDescriptor>()
                {
                    new AttributeDescriptor("level", hero.level),
                    new AttributeDescriptor("paragon", hero.paragonLevel)
                };
                progressListView.Adapter = new AttributesListAdapter(this, progressAttr.ToArray());

                ListView killsListView = FindViewById<ListView>(Resource.Id.killsLifetimeListView);
                List<AttributeDescriptor> killsAttr = new List<AttributeDescriptor>()
                {
                    new AttributeDescriptor("elites", hero.kills.elites)
                };
                killsListView.Adapter = new AttributesListAdapter(this, killsAttr.ToArray());

                ListView characteristicsListView = FindViewById<ListView>(Resource.Id.characteristicsListView);
                List<AttributeDescriptor> characteristicsAttr = new List<AttributeDescriptor>()
                {
                    new AttributeDescriptor("damage", hero.stats.damage),
                    new AttributeDescriptor("dexterity", hero.stats.dexterity),
                    new AttributeDescriptor("intelligence", hero.stats.intelligence),
                    new AttributeDescriptor("strength", hero.stats.strength),
                    new AttributeDescriptor("vitality", hero.stats.vitality),
                    new AttributeDescriptor("critic chance", hero.stats.critChance, true),
                    new AttributeDescriptor("critic damage increase", hero.stats.critDamage, true),
                    new AttributeDescriptor("life on hit", hero.stats.lifeOnHit),
                    new AttributeDescriptor("life steal", hero.stats.lifeSteal, true),
                    new AttributeDescriptor("life per kill", hero.stats.lifePerKill),
                    new AttributeDescriptor("armor", hero.stats.armor),
                    new AttributeDescriptor("arcane resist", hero.stats.arcaneResist),
                    new AttributeDescriptor("cold resist", hero.stats.coldResist),
                    new AttributeDescriptor("fire resist", hero.stats.fireResist),
                    new AttributeDescriptor("lightning resist", hero.stats.lightningResist),
                    new AttributeDescriptor("physical resist", hero.stats.physicalResist),
                    new AttributeDescriptor("poison resist", hero.stats.poisonResist),
                    new AttributeDescriptor("gold find", hero.stats.goldFind, true),
                    new AttributeDescriptor("magic find", hero.stats.magicFind, true),
                };
                characteristicsListView.Adapter = new AttributesListAdapter(this, characteristicsAttr.ToArray());
            }
        }
    }
}