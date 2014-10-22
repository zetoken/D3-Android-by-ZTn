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
        public UpdatableFragment FragmentCharacteristics;
        public UpdatableFragment FragmentComputed;
        public UpdatableFragment FragmentGear;
        public UpdatableFragment FragmentSkills;

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

            HasOptionsMenu = true;
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

            var progressDialog = ProgressDialog.Show(Activity, Resources.GetString(Resource.String.LoadingHero), Resources.GetString(Resource.String.WaitWhileRetrievingData), true);

            new Thread(() =>
            {
                try
                {
                    D3Context.Instance.CurrentHero = FetchHero(online);
                    Activity.RunOnUiThread(() => progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingItems)));
                    D3Context.Instance.CurrentHeroItems = FetchFullItems(online);

                    Activity.RunOnUiThread(() => progressDialog.SetTitle(Resources.GetString(Resource.String.LoadingIcons)));

                    // Icons are fetched with Online.OnlineIfMissing even if FetchMode.Online is asked
                    var fetchIconsOnlineMode = (online == FetchMode.Online ? FetchMode.OnlineIfMissing : online);
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
                        Toast.MakeText(Activity, "Hero not in cache" + Environment.NewLine + "Please use refresh action", ToastLength.Long)
                            .Show();
                    });
                }
                catch (Exception exception)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog.Dismiss();
                        Toast.MakeText(Activity, Resources.GetString(Resource.String.ErrorOccuredWhileRetrievingData), ToastLength.Long)
                            .Show();
                        Console.WriteLine(exception);
                    });
                }
            }).Start();
        }

        private static HeroItems FetchFullItems(FetchMode online)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var heroItems = D3Context.Instance.CurrentHero.Items;

            var dataProvider = (CacheableDataProvider)D3Api.DataProvider;
            dataProvider.FetchMode = online;

            try
            {
                // Compute set items bonus
                heroItems.UpdateToFullItems();

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
                items = items.Where(i => i != null)
                    .ToList();

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

            var icons = new IconsContainer();

            var dataProvider = (CacheableDataProvider)D3Api.DataProvider;
            dataProvider.FetchMode = online;

            try
            {
                icons.FetchItemIcons(D3Context.Instance.CurrentHero.Items);
                icons.FetchActiveSkillIcons(D3Context.Instance.CurrentHero.Skills.active);
                icons.FetchPassiveSkillIcons(D3Context.Instance.CurrentHero.Skills.passive);
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
                hero = Hero.CreateFromHeroId(new BattleTag(D3Context.Instance.BattleTag), D3Context.Instance.CurrentHeroSummary.Id);
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