﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTn.Pcl.D3Calculator.Annotations;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.Resources;
using ZTn.Pcl.D3Calculator.Views;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    public class HeroViewModel : INotifyPropertyChanged
    {
        private readonly Page _page;
        private readonly BnetAccount _account;
        private BindableTask<Hero> _hero;
        private readonly HeroSummary _heroSummary;
        private BindableTask<Item> _headItem;
        private BindableTask<Item> _bracersItem;

        public ObservableCollection<IControlData> BattleNetData { get; set; }

        public ObservableCollection<IControlData> ItemsData { get; set; }

        public ObservableCollection<IControlData> LegendaryPowersData { get; set; }

        public BindableTask<Hero> Hero
        {
            get { return _hero; }
            private set
            {
                _hero = value;
                OnPropertyChanged();
            }
        }

        public BindableTask<Item> BracersItem
        {
            get { return _bracersItem; }
            set { _bracersItem = value; OnPropertyChanged(); }
        }
        public BindableTask<Item> HeadItem
        {
            get { return _headItem; }
            set { _headItem = value; OnPropertyChanged(); }
        }

        public string HeroTitle => Lang.Hero.ToUpper();

        public BusyIndicatorViewModel BusyIndicatorLoadingHero { get; }

        #region >> Constructors

        public HeroViewModel(Page page, BnetAccount account, HeroSummary heroSummary)
        {
            _page = page;
            _account = account;
            _heroSummary = heroSummary;

            BusyIndicatorLoadingHero = new BusyIndicatorViewModel
            {
                IsBusy = true,
                BusyMessage = Lang.LoadingHero
            };
            BattleNetData = new ObservableCollection<IControlData>();

            ItemsData = new ObservableCollection<IControlData>
            {
                new TitleData(Lang.ItemHead),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Head),
                new TitleData(Lang.ItemBracers),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Bracers),
                new TitleData(Lang.ItemFeet),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Feet),
                new TitleData(Lang.ItemHands),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Hands),
                new TitleData(Lang.ItemLeftFinger),
                new ItemSummaryData(new ItemSummary(), ItemPosition.LeftFingers),
                new TitleData(Lang.ItemLegs),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Legs),
                new TitleData(Lang.ItemMainHand),
                new ItemSummaryData(new ItemSummary(), ItemPosition.MainHand),
                new TitleData(Lang.ItemNeck),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Neck),
                new TitleData(Lang.ItemOffHand),
                new ItemSummaryData(new ItemSummary(), ItemPosition.OffHand),
                new TitleData(Lang.ItemRightFinger),
                new ItemSummaryData(new ItemSummary(), ItemPosition.RightFinger),
                new TitleData(Lang.ItemShoulders),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Shoulders),
                new TitleData(Lang.ItemTorso),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Torso),
                new TitleData(Lang.ItemWaist),
                new ItemSummaryData(new ItemSummary(), ItemPosition.Waist)
            };

            LegendaryPowersData = new ObservableCollection<IControlData>
            {
                new TitleData(Lang.Weapon),
                new LegendaryPowerData(new LegendaryPower(), KanaiCubePosition.Weapon),
                new TitleData(Lang.Armor),
                new LegendaryPowerData(new LegendaryPower(), KanaiCubePosition.Armor),
                new TitleData(Lang.Jewelry),
                new LegendaryPowerData(new LegendaryPower(), KanaiCubePosition.Jewelry)
            };

            RefreshHero();
        }

        #endregion

        private void BuildBlizzard(Hero hero)
        {
            if (hero == null)
            {
                return;
            }

            BattleNetData.Clear();

            BattleNetData.Add(new TitleData(Lang.Progress));
            BattleNetData.Add(new TextData(Lang.LastUpdated, $"{hero.LastUpdated}"));
            BattleNetData.Add(new TextData(Lang.HeroClass, $"{hero.HeroClass}"));
            BattleNetData.Add(new ValueData(Lang.Level, hero.Level));
            BattleNetData.Add(new ValueData(Lang.ParagonLevel, hero.ParagonLevel));

            BattleNetData.Add(new TitleData(Lang.KillsLifetime));
            BattleNetData.Add(new ValueData(Lang.Elites, hero.Kills.Elites));

            BattleNetData.Add(new TitleData(Lang.Attributes));
            BattleNetData.Add(new ValueData(Lang.Dexterity, hero.Stats.Dexterity));
            BattleNetData.Add(new ValueData(Lang.Intelligence, hero.Stats.Intelligence));
            BattleNetData.Add(new ValueData(Lang.Strength, hero.Stats.Strength));
            BattleNetData.Add(new ValueData(Lang.Vitality, hero.Stats.Vitality));

            BattleNetData.Add(new TitleData(Lang.Damages));
            BattleNetData.Add(new ValueData(Lang.Damage, hero.Stats.Damage, 1));
            BattleNetData.Add(new PercentData(Lang.CriticChance, hero.Stats.CritChance, 1));
            BattleNetData.Add(new PercentData(Lang.CriticDamage, hero.Stats.CritDamage));
            BattleNetData.Add(new PercentData(Lang.AttackSpeed, hero.Stats.AttackSpeed, 2));

            BattleNetData.Add(new TitleData(Lang.Life));
            BattleNetData.Add(new ValueData(Lang.Healing, hero.Stats.Healing));
            BattleNetData.Add(new ValueData(Lang.Toughness, hero.Stats.Toughness));
            BattleNetData.Add(new ValueData(Lang.Life, hero.Stats.Life));
            BattleNetData.Add(new ValueData(Lang.LifeOnHit, hero.Stats.LifeOnHit));
            BattleNetData.Add(new ValueData(Lang.LifePerKill, hero.Stats.LifePerKill));
            BattleNetData.Add(new PercentData(Lang.LifeSteal, hero.Stats.LifeSteal));

            BattleNetData.Add(new TitleData(Lang.Defense));
            BattleNetData.Add(new ValueData(Lang.Armor, hero.Stats.Armor));
            BattleNetData.Add(new ValueData(Lang.ArcaneResist, hero.Stats.ArcaneResist));
            BattleNetData.Add(new ValueData(Lang.ColdResist, hero.Stats.ColdResist));
            BattleNetData.Add(new ValueData(Lang.FireResist, hero.Stats.FireResist));
            BattleNetData.Add(new ValueData(Lang.LightningResist, hero.Stats.LightningResist));
            BattleNetData.Add(new ValueData(Lang.PhysicalResist, hero.Stats.PhysicalResist));
            BattleNetData.Add(new ValueData(Lang.PoisonResist, hero.Stats.PoisonResist));

            BattleNetData.Add(new TitleData(Lang.Resources));
            BattleNetData.Add(new ValueData(Lang.PrimaryResource, hero.Stats.PrimaryResource));
            BattleNetData.Add(new ValueData(Lang.SecondaryResource, hero.Stats.SecondaryResource));

            BattleNetData.Add(new TitleData(Lang.Bonuses));
            BattleNetData.Add(new BonusPercentData(Lang.GoldFind, hero.Stats.GoldFind));
            BattleNetData.Add(new BonusPercentData(Lang.MagicFind, hero.Stats.MagicFind));
        }

        #region >> Hero handling

        public void RefreshHero(FetchMode fetchMode = FetchMode.OnlineIfMissing)
        {
            Hero = new BindableTask<Hero>(LoadHeroAsync(fetchMode));

            Hero.Task.ContinueWith(task =>
            {
                RefreshLegendaryPowers(task, fetchMode);
                RefreshItems(task, fetchMode);
            });
        }

        private async Task<Hero> LoadHeroAsync(FetchMode fetchMode)
        {
            BusyIndicatorLoadingHero.IsBusy = true;

            var d3Api = App.GetD3ApiRequester(_account.Host, fetchMode);

            Hero hero = null;
            try
            {
                hero = await d3Api.GetHeroFromHeroIdAsync(new BattleTag(_account.BattleTag), _heroSummary.Id);

                BuildBlizzard(hero);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);

                await _page.DisplayAlert(Lang.ApplicationName, Lang.NetworkError, Lang.Cancel);
            }
            finally
            {
                BusyIndicatorLoadingHero.IsBusy = false;
            }

            return hero;
        }

        #endregion

        #region >> Items handling

        private void RefreshItems(Task<Hero> heroTask, FetchMode fetchMode)
        {
            Hero hero = null;

            try
            {
                hero = heroTask.Result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            if (hero?.Items == null)
            {
                return;
            }

            RefreshItem(hero.Items.Bracers, ItemPosition.Bracers, fetchMode);
            RefreshItem(hero.Items.Feet, ItemPosition.Feet, fetchMode);
            RefreshItem(hero.Items.Hands, ItemPosition.Hands, fetchMode);
            RefreshItem(hero.Items.Head, ItemPosition.Head, fetchMode);
            RefreshItem(hero.Items.LeftFinger, ItemPosition.LeftFingers, fetchMode);
            RefreshItem(hero.Items.Legs, ItemPosition.Legs, fetchMode);
            RefreshItem(hero.Items.MainHand, ItemPosition.MainHand, fetchMode);
            RefreshItem(hero.Items.Neck, ItemPosition.Neck, fetchMode);
            RefreshItem(hero.Items.OffHand, ItemPosition.OffHand, fetchMode);
            RefreshItem(hero.Items.RightFinger, ItemPosition.RightFinger, fetchMode);
            RefreshItem(hero.Items.Shoulders, ItemPosition.Shoulders, fetchMode);
            RefreshItem(hero.Items.Torso, ItemPosition.Torso, fetchMode);
            RefreshItem(hero.Items.Waist, ItemPosition.Waist, fetchMode);
        }

        private void RefreshItem(ItemSummary itemSummary, ItemPosition position, FetchMode fetchMode)
        {
            var bindableItem = new BindableTask<Item>(LoadItemAsync(itemSummary, fetchMode));
            bindableItem.Task.ContinueWith(itemTask => UpdateItemInView(itemTask.Result, position));
        }

        private async Task<Item> LoadItemAsync(ItemSummary itemSummary, FetchMode fetchMode)
        {
            if (itemSummary?.TooltipParams == null)
            {
                return null;
            }

            var d3Api = App.GetD3ApiRequester(_account.Host, fetchMode);

            Item item = null;

            try
            {
                item = await d3Api.GetItemFromTooltipParamsAsync(itemSummary.TooltipParams);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);

                await _page.DisplayAlert(Lang.ApplicationName, Lang.NetworkError, Lang.Cancel);
            }

            return item;
        }

        private void UpdateItemInView(Item item, ItemPosition position)
        {
            if (item == null)
            {
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                var itemSummaryData = ItemsData.OfType<ItemSummaryData>().FirstOrDefault(i => i.Position == position);

                if (itemSummaryData == null)
                {
                    return;
                }

                var index = ItemsData.IndexOf(itemSummaryData);
                ItemsData[index] = new ItemData(item, position);
            });
        }

        #endregion

        #region >> Legendary Powers handling

        private void RefreshLegendaryPowers(Task<Hero> heroTask, FetchMode fetchMode)
        {
            Hero hero = null;

            try
            {
                hero = heroTask.Result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            if (hero?.LegendaryPowers == null || hero.LegendaryPowers.Length == 0)
            {
                return;
            }

            RefreshLegendaryPower(hero.LegendaryPowers[0], KanaiCubePosition.Weapon, fetchMode);
            RefreshLegendaryPower(hero.LegendaryPowers[1], KanaiCubePosition.Armor, fetchMode);
            RefreshLegendaryPower(hero.LegendaryPowers[2], KanaiCubePosition.Jewelry, fetchMode);
        }

        private void RefreshLegendaryPower(LegendaryPower itemSummary, KanaiCubePosition position, FetchMode fetchMode)
        {
            var bindableItem = new BindableTask<Item>(LoadLegendaryPowerAsync(itemSummary, fetchMode));
            bindableItem.Task.ContinueWith(itemTask => UpdateLegendaryPowerInView(itemTask.Result, position));
        }

        private async Task<Item> LoadLegendaryPowerAsync(LegendaryPower legendaryPower, FetchMode fetchMode)
        {
            if (legendaryPower?.TooltipParams == null)
            {
                return null;
            }

            var d3Api = App.GetD3ApiRequester(_account.Host, fetchMode);

            var item = await d3Api.GetItemFromTooltipParamsAsync(legendaryPower.TooltipParams);

            return item;
        }

        private void UpdateLegendaryPowerInView(Item item, KanaiCubePosition position)
        {
            if (item == null)
            {
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                var legendaryPowerData =
                    LegendaryPowersData.OfType<LegendaryPowerData>().FirstOrDefault(i => i.Position == position);

                if (legendaryPowerData == null)
                {
                    return;
                }

                var index = ItemsData.IndexOf(legendaryPowerData);
                // TODO ItemsData[index] = new LegendaryPowerItemData(item, position);
            });
        }

        #endregion

        #region >> INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
