using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Calculator.Sets;
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
        ActionBar.Tab tabComputed;
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

            tabComputed = ActionBar
                .NewTab()
                .SetText(Resources.GetString(Resource.String.computed))
                .SetTabListener(new SimpleTabListener<HeroComputedListFragment>(this));
            ActionBar.AddTab(tabComputed);

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
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingItems));
                    });
                    D3Context.getInstance().heroItems = fetchFullItems(online);
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingIcons));
                    });
                    D3Context.getInstance().icons = fetchIcons(online);

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
                if (heroItems.offHand != null)
                    heroItems.offHand = heroItems.offHand.getFullItem();
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

                KnownSets knownSets = KnownSets.getKnownSetFromJSonStream(this.Assets.Open("d3set.json"));
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
                dataProvider.online = D3Context.getInstance().onlineMode;
            }

            return heroItems;
        }

        private IconsContainer fetchIcons(Boolean online)
        {
            HeroItems heroItems = D3Context.getInstance().hero.items;
            HeroSkills skills = D3Context.getInstance().hero.skills;
            IconsContainer icons = new IconsContainer();

            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.online = online;

            try
            {
                if (heroItems.head != null)
                    icons.head = D3Api.getItemIcon(heroItems.head.icon);
                if (heroItems.torso != null)
                    icons.torso = D3Api.getItemIcon(heroItems.torso.icon);
                if (heroItems.feet != null)
                    icons.feet = D3Api.getItemIcon(heroItems.feet.icon);
                if (heroItems.hands != null)
                    icons.hands = D3Api.getItemIcon(heroItems.hands.icon);
                if (heroItems.shoulders != null)
                    icons.shoulders = D3Api.getItemIcon(heroItems.shoulders.icon);
                if (heroItems.legs != null)
                    icons.legs = D3Api.getItemIcon(heroItems.legs.icon);
                if (heroItems.bracers != null)
                    icons.bracers = D3Api.getItemIcon(heroItems.bracers.icon);
                if (heroItems.mainHand != null)
                    icons.mainHand = D3Api.getItemIcon(heroItems.mainHand.icon);
                if (heroItems.offHand != null)
                    icons.offHand = D3Api.getItemIcon(heroItems.offHand.icon);
                if (heroItems.waist != null)
                    icons.waist = D3Api.getItemIcon(heroItems.waist.icon);
                if (heroItems.rightFinger != null)
                    icons.rightFinger = D3Api.getItemIcon(heroItems.rightFinger.icon);
                if (heroItems.leftFinger != null)
                    icons.leftFinger = D3Api.getItemIcon(heroItems.leftFinger.icon);
                if (heroItems.neck != null)
                    icons.neck = D3Api.getItemIcon(heroItems.neck.icon);

                if (skills.active[0] != null && skills.active[0].skill != null)
                    icons.activeSkill1 = D3Api.getSkillIcon(skills.active[0].skill.icon);
                if (skills.active[1] != null && skills.active[1].skill != null)
                    icons.activeSkill2 = D3Api.getSkillIcon(skills.active[1].skill.icon);
                if (skills.active[2] != null && skills.active[2].skill != null)
                    icons.activeSkill3 = D3Api.getSkillIcon(skills.active[2].skill.icon);
                if (skills.active[3] != null && skills.active[3].skill != null)
                    icons.activeSkill4 = D3Api.getSkillIcon(skills.active[3].skill.icon);
                if (skills.active[4] != null && skills.active[3].skill != null)
                    icons.activeSkill5 = D3Api.getSkillIcon(skills.active[4].skill.icon);
                if (skills.active[5] != null && skills.active[3].skill != null)
                    icons.activeSkill6 = D3Api.getSkillIcon(skills.active[5].skill.icon);

                if (skills.passive[0] != null && skills.passive[0].skill != null)
                    icons.passiveSkill1 = D3Api.getSkillIcon(skills.passive[0].skill.icon);
                if (skills.passive[1] != null && skills.passive[1].skill != null)
                    icons.passiveSkill2 = D3Api.getSkillIcon(skills.passive[1].skill.icon);
                if (skills.passive[2] != null && skills.passive[2].skill != null)
                    icons.passiveSkill3 = D3Api.getSkillIcon(skills.passive[2].skill.icon);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                D3Context.getInstance().icons = null;
                throw exception;
            }
            finally
            {
                dataProvider.online = D3Context.getInstance().onlineMode;
            }

            return icons;
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