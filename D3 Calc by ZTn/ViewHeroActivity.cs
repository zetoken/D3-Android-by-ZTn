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
using ZTn.BNet.D3.Items;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "View Hero", Theme = "@android:style/Theme.Holo", Icon = "@drawable/icon")]
    public class ViewHeroActivity : Activity
    {
        String battleTag;
        String host;
        HeroSummary heroSummary;

        ActionBar.Tab tabCharacteristics;
        ActionBar.Tab tabGear;
        ActionBar.Tab tabSkills;

        protected override void OnCreate(Bundle bundle)
        {
            Console.WriteLine("ViewHeroActivity: OnCreate");
            base.OnCreate(bundle);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;
            heroSummary = D3Context.getInstance().heroSummary;

            SetContentView(Resource.Layout.FragmentContainer);

            this.Title = D3Context.getInstance().heroSummary.name;
            this.ActionBar.Subtitle = D3Context.getInstance().battleTag;

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            tabCharacteristics = ActionBar
                .NewTab()
                .SetText(Resources.GetString(Resource.String.details))
                .SetTabListener(new SimpleTabListener<HeroCharacteristicsListFragment>(this));
            ActionBar.AddTab(tabCharacteristics);

            tabSkills = ActionBar
                .NewTab()
                .SetText(Resources.GetString(Resource.String.skills))
                .SetTabListener(new SimpleTabListener<HeroSkillsListFragment>(this));
            ActionBar.AddTab(tabSkills);

            tabGear = ActionBar
                .NewTab()
                .SetText(Resources.GetString(Resource.String.gear))
                .SetTabListener(new SimpleTabListener<HeroGearListFragment>(this));
            ActionBar.AddTab(tabGear);

            D3Context.getInstance().hero = null;
            deferredFetchHero(D3Context.getInstance().onlineMode);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ViewHeroActivity, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.RefreshContent:
                    deferredFetchHero(true);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void deferredFetchHero(Boolean online)
        {
            ProgressDialog progressDialog = null;

            if (online)
                progressDialog = ProgressDialog.Show(this, Resources.GetString(Resource.String.LoadingHero), Resources.GetString(Resource.String.WaitWhileRetrievingData), true);

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    D3Context.getInstance().hero = fetchHero(online);
                    D3Context.getInstance().heroItems = fetchFullItems(online);
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        ActionBar.SelectTab(tabCharacteristics);
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, "Hero not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, Resources.GetString(Resource.String.ErrorOccuredWhileRetrievingData), ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            })).Start();
        }

        private HeroItems fetchFullItems(Boolean online)
        {
            HeroItems heroItems = D3Context.getInstance().hero.items;

            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.online = online;

            try
            {
                if (heroItems.head != null)
                {
                    heroItems.head = Item.getItemFromTooltipParams(heroItems.head.tooltipParams);
                    D3Context.getInstance().headIcon = D3Api.getItemIcon(heroItems.head.icon);
                }
                if (heroItems.torso != null)
                {
                    heroItems.torso = Item.getItemFromTooltipParams(heroItems.torso.tooltipParams);
                    D3Context.getInstance().torsoIcon = D3Api.getItemIcon(heroItems.torso.icon);
                }
                if (heroItems.feet != null)
                {
                    heroItems.feet = Item.getItemFromTooltipParams(heroItems.feet.tooltipParams);
                    D3Context.getInstance().feetIcon = D3Api.getItemIcon(heroItems.feet.icon);
                }
                if (heroItems.hands != null)
                {
                    heroItems.hands = Item.getItemFromTooltipParams(heroItems.hands.tooltipParams);
                    D3Context.getInstance().handsIcon = D3Api.getItemIcon(heroItems.hands.icon);
                }
                if (heroItems.shoulders != null)
                {
                    heroItems.shoulders = Item.getItemFromTooltipParams(heroItems.shoulders.tooltipParams);
                    D3Context.getInstance().shouldersIcon = D3Api.getItemIcon(heroItems.shoulders.icon);
                }
                if (heroItems.legs != null)
                {
                    heroItems.legs = Item.getItemFromTooltipParams(heroItems.legs.tooltipParams);
                    D3Context.getInstance().legsIcon = D3Api.getItemIcon(heroItems.legs.icon);
                }
                if (heroItems.bracers != null)
                {
                    heroItems.bracers = Item.getItemFromTooltipParams(heroItems.bracers.tooltipParams);
                    D3Context.getInstance().bracersIcon = D3Api.getItemIcon(heroItems.bracers.icon);
                }
                if (heroItems.mainHand != null)
                {
                    heroItems.mainHand = Item.getItemFromTooltipParams(heroItems.mainHand.tooltipParams);
                    D3Context.getInstance().mainHandIcon = D3Api.getItemIcon(heroItems.mainHand.icon);
                }
                if (heroItems.offHand != null)
                {
                    heroItems.offHand = Item.getItemFromTooltipParams(heroItems.offHand.tooltipParams);
                    D3Context.getInstance().offHandIcon = D3Api.getItemIcon(heroItems.offHand.icon);
                }
                if (heroItems.waist != null)
                {
                    heroItems.waist = Item.getItemFromTooltipParams(heroItems.waist.tooltipParams);
                    D3Context.getInstance().waistIcon = D3Api.getItemIcon(heroItems.waist.icon);
                }
                if (heroItems.rightFinger != null)
                {
                    heroItems.rightFinger = Item.getItemFromTooltipParams(heroItems.rightFinger.tooltipParams);
                    D3Context.getInstance().rightFingerIcon = D3Api.getItemIcon(heroItems.rightFinger.icon);
                }
                if (heroItems.leftFinger != null)
                {
                    heroItems.leftFinger = Item.getItemFromTooltipParams(heroItems.leftFinger.tooltipParams);
                    D3Context.getInstance().leftFingerIcon = D3Api.getItemIcon(heroItems.leftFinger.icon);
                }
                if (heroItems.neck != null)
                {
                    heroItems.neck = Item.getItemFromTooltipParams(heroItems.neck.tooltipParams);
                    D3Context.getInstance().neckIcon = D3Api.getItemIcon(heroItems.neck.icon);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                D3Context.getInstance().heroItems = null;
                throw exception;
            }
            finally
            {
                dataProvider.online = D3Context.getInstance().onlineMode;
            }

            return heroItems;
        }

        private Hero fetchHero(Boolean online)
        {
            Console.WriteLine("ViewHeroActivity: fetchHero");
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
    }
}