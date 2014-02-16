using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Calculator.Helpers;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;

using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using CacheableDataProvider = ZTnDroid.D3Calculator.DataProviders.CacheableDataProvider;
using Environment = System.Environment;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class FetchHeroFragment : Fragment
    {
        public HeroCharacteristicsListFragment FragmentCharacteristics;
        public HeroComputedListFragment FragmentComputed;
        public HeroGearListFragment FragmentGear;
        public HeroSkillsListFragment FragmentSkills;

        #region >> Constructors

        public FetchHeroFragment()
        {
            FragmentCharacteristics = new HeroCharacteristicsListFragment();
            FragmentComputed = new HeroComputedListFragment();
            FragmentSkills = new HeroSkillsListFragment();
            FragmentGear = new HeroGearListFragment();
        }

        #endregion

        #region >> Fragment

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            RetainInstance = true;

            SetHasOptionsMenu(true);
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            // Fetch hero from server
            D3Context.Instance.CurrentHero = null;
            DeferredFetchHero(D3Context.Instance.FetchMode);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        /// <inheritdoc/>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreateOptionsMenu(menu, inflater);

            inflater.Inflate(Resource.Menu.ViewHeroActivity, menu);

        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.RefreshContent:
                    DeferredFetchHero(FetchMode.Online);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

        private void DeferredFetchHero(FetchMode online)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            ProgressDialog progressDialog = ProgressDialog.Show(Activity, Resources.GetString(Resource.String.LoadingHero), Resources.GetString(Resource.String.WaitWhileRetrievingData), true);

            new Thread(() =>
            {
                try
                {
                    D3Context.Instance.CurrentHero = FetchHero(online);
                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingItems));
                    });
                    D3Context.Instance.CurrentHeroItems = FetchFullItems(online);

                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingIcons));
                    });

                    // Icons are fetched with Online.OnlineIfMissing even if FetchMode.Online is asked
                    FetchMode fetchIconsOnlineMode;
                    if (online == FetchMode.Online)
                        fetchIconsOnlineMode = FetchMode.OnlineIfMissing;
                    else
                        fetchIconsOnlineMode = online;
                    D3Context.Instance.Icons = FetchIcons(fetchIconsOnlineMode);

                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog.Dismiss();
                        FragmentCharacteristics.UpdateFragment();
                        FragmentComputed.UpdateFragment();
                        FragmentSkills.UpdateFragment();
                        FragmentGear.UpdateFragment();
                    });
                }
                catch (FileNotInCacheException)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog.Dismiss();
                        Toast.MakeText(Activity, "Hero not in cache" + Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog.Dismiss();
                        Toast.MakeText(Activity, Resources.GetString(Resource.String.ErrorOccuredWhileRetrievingData), ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            }).Start();
        }

        private static HeroItems FetchFullItems(FetchMode online)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            HeroItems heroItems = D3Context.Instance.CurrentHero.items;

            var dataProvider = (CacheableDataProvider)D3Api.DataProvider;
            dataProvider.FetchMode = online;

            try
            {
                if (heroItems.head != null)
                    heroItems.head = heroItems.head.GetFullItem();
                if (heroItems.torso != null)
                    heroItems.torso = heroItems.torso.GetFullItem();
                if (heroItems.feet != null)
                    heroItems.feet = heroItems.feet.GetFullItem();
                if (heroItems.hands != null)
                    heroItems.hands = heroItems.hands.GetFullItem();
                if (heroItems.shoulders != null)
                    heroItems.shoulders = heroItems.shoulders.GetFullItem();
                if (heroItems.legs != null)
                    heroItems.legs = heroItems.legs.GetFullItem();
                if (heroItems.bracers != null)
                    heroItems.bracers = heroItems.bracers.GetFullItem();
                if (heroItems.mainHand != null)
                    heroItems.mainHand = heroItems.mainHand.GetFullItem();
                else
                    heroItems.mainHand = ZTn.BNet.D3.Calculator.D3Calculator.NakedHandWeapon;
                if (heroItems.offHand != null)
                    heroItems.offHand = heroItems.offHand.GetFullItem();
                else
                    heroItems.offHand = ZTn.BNet.D3.Calculator.D3Calculator.BlankWeapon;
                if (heroItems.waist != null)
                    heroItems.waist = heroItems.waist.GetFullItem();
                if (heroItems.rightFinger != null)
                    heroItems.rightFinger = heroItems.rightFinger.GetFullItem();
                if (heroItems.leftFinger != null)
                    heroItems.leftFinger = heroItems.leftFinger.GetFullItem();
                if (heroItems.neck != null)
                    heroItems.neck = heroItems.neck.GetFullItem();

                // Compute set items bonus
                var items = new List<Item>
                {
                    (Item)heroItems.bracers,
                    (Item)heroItems.feet,
                    (Item)heroItems.hands,
                    (Item)heroItems.head,
                    (Item)heroItems.leftFinger,
                    (Item)heroItems.legs,
                    (Item)heroItems.neck,
                    (Item)heroItems.rightFinger,
                    (Item)heroItems.shoulders,
                    (Item)heroItems.torso,
                    (Item)heroItems.waist,
                    (Item)heroItems.mainHand,
                    (Item)heroItems.offHand
                };
                items = items.Where(i => i != null).ToList();

                D3Context.Instance.ActivatedSetBonus = new Item(items.GetActivatedSetBonus());
                D3Context.Instance.ActivatedSets = items.GetActivatedSets();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                D3Context.Instance.CurrentHeroItems = null;
                D3Context.Instance.ActivatedSetBonus = null;
                throw;
            }
            finally
            {
                dataProvider.FetchMode = D3Context.Instance.FetchMode;
            }

            return heroItems;
        }

        private static IconsContainer FetchIcons(FetchMode online)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var heroItems = D3Context.Instance.CurrentHero.items;
            var skills = D3Context.Instance.CurrentHero.skills;
            var icons = new IconsContainer();

            var dataProvider = (CacheableDataProvider)D3Api.DataProvider;
            dataProvider.FetchMode = online;

            try
            {
                if (heroItems.head != null && heroItems.head.icon != null)
                    icons.Head = D3Api.GetItemIcon(heroItems.head.icon, "large");
                if (heroItems.torso != null && heroItems.torso.icon != null)
                    icons.Torso = D3Api.GetItemIcon(heroItems.torso.icon, "large");
                if (heroItems.feet != null && heroItems.feet.icon != null)
                    icons.Feet = D3Api.GetItemIcon(heroItems.feet.icon, "large");
                if (heroItems.hands != null && heroItems.hands.icon != null)
                    icons.Hands = D3Api.GetItemIcon(heroItems.hands.icon, "large");
                if (heroItems.shoulders != null && heroItems.shoulders.icon != null)
                    icons.Shoulders = D3Api.GetItemIcon(heroItems.shoulders.icon, "large");
                if (heroItems.legs != null && heroItems.legs.icon != null)
                    icons.Legs = D3Api.GetItemIcon(heroItems.legs.icon, "large");
                if (heroItems.bracers != null && heroItems.bracers.icon != null)
                    icons.Bracers = D3Api.GetItemIcon(heroItems.bracers.icon, "large");
                if (heroItems.mainHand != null && heroItems.mainHand.icon != null)
                    icons.MainHand = D3Api.GetItemIcon(heroItems.mainHand.icon, "large");
                if (heroItems.offHand != null && heroItems.offHand.icon != null)
                    icons.OffHand = D3Api.GetItemIcon(heroItems.offHand.icon, "large");
                if (heroItems.waist != null && heroItems.waist.icon != null)
                    icons.Waist = D3Api.GetItemIcon(heroItems.waist.icon, "large");
                if (heroItems.rightFinger != null && heroItems.rightFinger.icon != null)
                    icons.RightFinger = D3Api.GetItemIcon(heroItems.rightFinger.icon, "large");
                if (heroItems.leftFinger != null && heroItems.leftFinger.icon != null)
                    icons.LeftFinger = D3Api.GetItemIcon(heroItems.leftFinger.icon, "large");
                if (heroItems.neck != null && heroItems.neck.icon != null)
                    icons.Neck = D3Api.GetItemIcon(heroItems.neck.icon, "large");

                if (skills.active[0] != null && skills.active[0].skill != null)
                    icons.ActiveSkill1 = D3Api.GetSkillIcon(skills.active[0].skill.icon, "64");
                if (skills.active[1] != null && skills.active[1].skill != null)
                    icons.ActiveSkill2 = D3Api.GetSkillIcon(skills.active[1].skill.icon, "64");
                if (skills.active[2] != null && skills.active[2].skill != null)
                    icons.ActiveSkill3 = D3Api.GetSkillIcon(skills.active[2].skill.icon, "64");
                if (skills.active[3] != null && skills.active[3].skill != null)
                    icons.ActiveSkill4 = D3Api.GetSkillIcon(skills.active[3].skill.icon, "64");
                if (skills.active[4] != null && skills.active[4].skill != null)
                    icons.ActiveSkill5 = D3Api.GetSkillIcon(skills.active[4].skill.icon, "64");
                if (skills.active[5] != null && skills.active[5].skill != null)
                    icons.ActiveSkill6 = D3Api.GetSkillIcon(skills.active[5].skill.icon, "64");

                if (skills.passive[0] != null && skills.passive[0].skill != null)
                    icons.PassiveSkill1 = D3Api.GetSkillIcon(skills.passive[0].skill.icon, "64");
                if (skills.passive[1] != null && skills.passive[1].skill != null)
                    icons.PassiveSkill2 = D3Api.GetSkillIcon(skills.passive[1].skill.icon, "64");
                if (skills.passive[2] != null && skills.passive[2].skill != null)
                    icons.PassiveSkill3 = D3Api.GetSkillIcon(skills.passive[2].skill.icon, "64");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                D3Context.Instance.Icons = null;
                throw;
            }
            finally
            {
                dataProvider.FetchMode = D3Context.Instance.FetchMode;
            }

            return icons;
        }

        private static Hero FetchHero(FetchMode online)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            Hero hero;

            D3Api.Host = D3Context.Instance.Host;
            var dataProvider = (CacheableDataProvider)D3Api.DataProvider;
            dataProvider.FetchMode = online;

            try
            {
                hero = Hero.CreateFromHeroId(new BattleTag(D3Context.Instance.BattleTag), D3Context.Instance.CurrentHeroSummary.id);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                hero = null;
            }
            finally
            {
                dataProvider.FetchMode = D3Context.Instance.FetchMode;
            }

            return hero;
        }
    }
}