using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Calculator.Sets;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class FetchHeroFragment : Fragment
    {
        String battleTag;
        String host;
        HeroSummary heroSummary;

        public HeroCharacteristicsListFragment fragmentCharacteristics;
        public HeroComputedListFragment fragmentComputed;
        public HeroGearListFragment fragmentGear;
        public HeroSkillsListFragment fragmentSkills;

        public FetchHeroFragment()
        {
            fragmentCharacteristics = new HeroCharacteristicsListFragment();
            fragmentComputed = new HeroComputedListFragment();
            fragmentSkills = new HeroSkillsListFragment();
            fragmentGear = new HeroGearListFragment();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            RetainInstance = true;

            SetHasOptionsMenu(true);

            // Fragment initialization
            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;
            heroSummary = D3Context.getInstance().heroSummary;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            // Fetch hero from server
            D3Context.getInstance().hero = null;
            deferredFetchHero(D3Context.getInstance().onlineMode);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void deferredFetchHero(OnlineMode online)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            ProgressDialog progressDialog = null;

            if (online == OnlineMode.Online)
                progressDialog = ProgressDialog.Show(this.Activity, Resources.GetString(Resource.String.LoadingHero), Resources.GetString(Resource.String.WaitWhileRetrievingData), true);

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    D3Context.getInstance().hero = fetchHero(online);
                    this.Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingItems));
                    });
                    D3Context.getInstance().heroItems = fetchFullItems(online);

                    this.Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingIcons));
                    });

                    // Icons are fetches with Online.OnlineIfMissing even if OnlineMode.Online is asked
                    OnlineMode fetchIconsOnlineMode;
                    if (online == OnlineMode.Online)
                        fetchIconsOnlineMode = OnlineMode.OnlineIfMissing;
                    else
                        fetchIconsOnlineMode = online;
                    D3Context.getInstance().icons = fetchIcons(fetchIconsOnlineMode);

                    this.Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.Dismiss();
                        fragmentCharacteristics.updateFragment();
                        fragmentComputed.updateFragment();
                        fragmentSkills.updateFragment();
                        fragmentGear.updateFragment();
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    this.Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this.Activity, "Hero not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    this.Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this.Activity, Resources.GetString(Resource.String.ErrorOccuredWhileRetrievingData), ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            })).Start();
        }

        private HeroItems fetchFullItems(OnlineMode online)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            HeroItems heroItems = D3Context.getInstance().hero.items;

            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.onlineMode = online;

            try
            {
                if (heroItems.head != null)
                    heroItems.head = heroItems.head.getFullItem();
                if (heroItems.torso != null)
                    heroItems.torso = heroItems.torso.getFullItem();
                if (heroItems.feet != null)
                    heroItems.feet = heroItems.feet.getFullItem();
                if (heroItems.hands != null)
                    heroItems.hands = heroItems.hands.getFullItem();
                if (heroItems.shoulders != null)
                    heroItems.shoulders = heroItems.shoulders.getFullItem();
                if (heroItems.legs != null)
                    heroItems.legs = heroItems.legs.getFullItem();
                if (heroItems.bracers != null)
                    heroItems.bracers = heroItems.bracers.getFullItem();
                if (heroItems.mainHand != null)
                    heroItems.mainHand = heroItems.mainHand.getFullItem();
                else
                    heroItems.mainHand = ZTn.BNet.D3.Calculator.D3Calculator.nakedHandWeapon;
                if (heroItems.offHand != null)
                    heroItems.offHand = heroItems.offHand.getFullItem();
                else
                    heroItems.offHand = ZTn.BNet.D3.Calculator.D3Calculator.blankWeapon;
                if (heroItems.waist != null)
                    heroItems.waist = heroItems.waist.getFullItem();
                if (heroItems.rightFinger != null)
                    heroItems.rightFinger = heroItems.rightFinger.getFullItem();
                if (heroItems.leftFinger != null)
                    heroItems.leftFinger = heroItems.leftFinger.getFullItem();
                if (heroItems.neck != null)
                    heroItems.neck = heroItems.neck.getFullItem();

                // Compute set items bonus
                List<Item> items = new List<Item>();
                if (heroItems.bracers != null)
                    items.Add((Item)heroItems.bracers);
                if (heroItems.feet != null)
                    items.Add((Item)heroItems.feet);
                if (heroItems.hands != null)
                    items.Add((Item)heroItems.hands);
                if (heroItems.head != null)
                    items.Add((Item)heroItems.head);
                if (heroItems.leftFinger != null)
                    items.Add((Item)heroItems.leftFinger);
                if (heroItems.legs != null)
                    items.Add((Item)heroItems.legs);
                if (heroItems.neck != null)
                    items.Add((Item)heroItems.neck);
                if (heroItems.rightFinger != null)
                    items.Add((Item)heroItems.rightFinger);
                if (heroItems.shoulders != null)
                    items.Add((Item)heroItems.shoulders);
                if (heroItems.torso != null)
                    items.Add((Item)heroItems.torso);
                if (heroItems.waist != null)
                    items.Add((Item)heroItems.waist);
                if (heroItems.mainHand != null)
                    items.Add((Item)heroItems.mainHand);
                if (heroItems.offHand != null)
                    items.Add((Item)heroItems.offHand);

                KnownSets knownSets = KnownSets.getKnownSetsFromJSonStream(this.Activity.Assets.Open("d3set.json"));
                D3Context.getInstance().setBonus = new Item(knownSets.getActivatedSetBonus(items));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                D3Context.getInstance().heroItems = null;
                D3Context.getInstance().setBonus = null;
                throw exception;
            }
            finally
            {
                dataProvider.onlineMode = D3Context.getInstance().onlineMode;
            }

            return heroItems;
        }

        private IconsContainer fetchIcons(OnlineMode online)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            HeroItems heroItems = D3Context.getInstance().hero.items;
            HeroSkills skills = D3Context.getInstance().hero.skills;
            IconsContainer icons = new IconsContainer();

            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.onlineMode = online;

            try
            {
                if (heroItems.head != null && heroItems.head.icon != null)
                    icons.head = D3Api.getItemIcon(heroItems.head.icon, "large");
                if (heroItems.torso != null && heroItems.torso.icon != null)
                    icons.torso = D3Api.getItemIcon(heroItems.torso.icon, "large");
                if (heroItems.feet != null && heroItems.feet.icon != null)
                    icons.feet = D3Api.getItemIcon(heroItems.feet.icon, "large");
                if (heroItems.hands != null && heroItems.hands.icon != null)
                    icons.hands = D3Api.getItemIcon(heroItems.hands.icon, "large");
                if (heroItems.shoulders != null && heroItems.shoulders.icon != null)
                    icons.shoulders = D3Api.getItemIcon(heroItems.shoulders.icon, "large");
                if (heroItems.legs != null && heroItems.legs.icon != null)
                    icons.legs = D3Api.getItemIcon(heroItems.legs.icon, "large");
                if (heroItems.bracers != null && heroItems.bracers.icon != null)
                    icons.bracers = D3Api.getItemIcon(heroItems.bracers.icon, "large");
                if (heroItems.mainHand != null && heroItems.mainHand.icon != null)
                    icons.mainHand = D3Api.getItemIcon(heroItems.mainHand.icon, "large");
                if (heroItems.offHand != null && heroItems.offHand.icon != null)
                    icons.offHand = D3Api.getItemIcon(heroItems.offHand.icon, "large");
                if (heroItems.waist != null && heroItems.waist.icon != null)
                    icons.waist = D3Api.getItemIcon(heroItems.waist.icon, "large");
                if (heroItems.rightFinger != null && heroItems.rightFinger.icon != null)
                    icons.rightFinger = D3Api.getItemIcon(heroItems.rightFinger.icon, "large");
                if (heroItems.leftFinger != null && heroItems.leftFinger.icon != null)
                    icons.leftFinger = D3Api.getItemIcon(heroItems.leftFinger.icon, "large");
                if (heroItems.neck != null && heroItems.neck.icon != null)
                    icons.neck = D3Api.getItemIcon(heroItems.neck.icon, "large");

                if (skills.active[0] != null && skills.active[0].skill != null)
                    icons.activeSkill1 = D3Api.getSkillIcon(skills.active[0].skill.icon, "64");
                if (skills.active[1] != null && skills.active[1].skill != null)
                    icons.activeSkill2 = D3Api.getSkillIcon(skills.active[1].skill.icon, "64");
                if (skills.active[2] != null && skills.active[2].skill != null)
                    icons.activeSkill3 = D3Api.getSkillIcon(skills.active[2].skill.icon, "64");
                if (skills.active[3] != null && skills.active[3].skill != null)
                    icons.activeSkill4 = D3Api.getSkillIcon(skills.active[3].skill.icon, "64");
                if (skills.active[4] != null && skills.active[4].skill != null)
                    icons.activeSkill5 = D3Api.getSkillIcon(skills.active[4].skill.icon, "64");
                if (skills.active[5] != null && skills.active[5].skill != null)
                    icons.activeSkill6 = D3Api.getSkillIcon(skills.active[5].skill.icon, "64");

                if (skills.passive[0] != null && skills.passive[0].skill != null)
                    icons.passiveSkill1 = D3Api.getSkillIcon(skills.passive[0].skill.icon, "64");
                if (skills.passive[1] != null && skills.passive[1].skill != null)
                    icons.passiveSkill2 = D3Api.getSkillIcon(skills.passive[1].skill.icon, "64");
                if (skills.passive[2] != null && skills.passive[2].skill != null)
                    icons.passiveSkill3 = D3Api.getSkillIcon(skills.passive[2].skill.icon, "64");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                D3Context.getInstance().icons = null;
                throw exception;
            }
            finally
            {
                dataProvider.onlineMode = D3Context.getInstance().onlineMode;
            }

            return icons;
        }

        private Hero fetchHero(OnlineMode online)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            Hero hero = null;

            D3Api.host = host;
            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.onlineMode = online;

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
                dataProvider.onlineMode = D3Context.getInstance().onlineMode;
            }

            return hero;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreateOptionsMenu(menu, inflater);

            inflater.Inflate(Resource.Menu.ViewHeroActivity, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.RefreshContent:
                    deferredFetchHero(OnlineMode.Online);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}