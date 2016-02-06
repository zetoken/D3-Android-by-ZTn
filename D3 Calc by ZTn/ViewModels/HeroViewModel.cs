using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Annotations;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.Resources;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    public class HeroViewModel : INotifyPropertyChanged
    {
        private readonly BnetAccount _account;
        private BindableTask<Hero> _hero;
        private readonly HeroSummary _heroSummary;

        public ObservableCollection<IListViewRowData> BattleNetData { get; set; }

        public BindableTask<Hero> Hero
        {
            get { return _hero; }
            private set
            {
                _hero = value;
                OnPropertyChanged();
            }
        }

        public HeroViewModel ViewModel => this;

        public string HeroTitle => Resources.Lang.Hero.ToUpper();

        public BusyIndicatorViewModel BusyIndicatorLoadingHero { get; }

        #region >> Constructors

        public HeroViewModel(BnetAccount account, HeroSummary heroSummary)
        {
            _account = account;
            _heroSummary = heroSummary;

            BusyIndicatorLoadingHero = new BusyIndicatorViewModel
            {
                IsBusy = true,
                BusyMessage = Lang.LoadingHero
            };
            BattleNetData = new ObservableCollection<IListViewRowData>();

            RefreshHero();
        }

        #endregion

        private async Task<Hero> LoadHeroAsync(FetchMode fetchMode)
        {
            BusyIndicatorLoadingHero.IsBusy = true;

            var dataProvider = DependencyService.Get<CacheableDataProvider>();
            dataProvider.FetchMode = fetchMode;

            var d3Api = new D3ApiRequester
            {
                ApiKey = App.ApiKey,
                DataProvider = dataProvider,
                Host = _account.Host
            };

            var hero = await d3Api.GetHeroFromHeroIdAsync(new BattleTag(_account.BattleTag), _heroSummary.Id);

            BusyIndicatorLoadingHero.IsBusy = false;

            BuildBlizzard(hero);

            return hero;
        }

        private void BuildBlizzard(Hero hero)
        {
            if (hero == null)
            {
                return;
            }

            BattleNetData.Clear();

            BattleNetData.Add(new TitleListViewData(Resources.Lang.Progress));
            BattleNetData.Add(new TextListViewData(Resources.Lang.LastUpdated, $"{hero.LastUpdated}"));
            BattleNetData.Add(new TextListViewData(Resources.Lang.HeroClass, $"{hero.HeroClass}"));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Level, hero.Level));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.ParagonLevel, hero.ParagonLevel));

            BattleNetData.Add(new TitleListViewData(Resources.Lang.KillsLifetime));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Elites, hero.Kills.Elites));

            BattleNetData.Add(new TitleListViewData(Resources.Lang.Attributes));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Dexterity, hero.Stats.Dexterity));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Intelligence, hero.Stats.Intelligence));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Strength, hero.Stats.Strength));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Vitality, hero.Stats.Vitality));

            BattleNetData.Add(new TitleListViewData(Resources.Lang.Damages));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Damage, hero.Stats.Damage, 1));
            BattleNetData.Add(new PercentListViewData(Resources.Lang.CriticChance, hero.Stats.CritChance, 1)); ;
            BattleNetData.Add(new PercentListViewData(Resources.Lang.CriticDamage, hero.Stats.CritDamage));
            BattleNetData.Add(new PercentListViewData(Resources.Lang.AttackSpeed, hero.Stats.AttackSpeed, 2));

            BattleNetData.Add(new TitleListViewData(Resources.Lang.Life));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Healing, hero.Stats.Healing));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Toughness, hero.Stats.Toughness));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Life, hero.Stats.Life));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.LifeOnHit, hero.Stats.LifeOnHit));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.LifePerKill, hero.Stats.LifePerKill));
            BattleNetData.Add(new PercentListViewData(Resources.Lang.LifeSteal, hero.Stats.LifeSteal));

            BattleNetData.Add(new TitleListViewData(Resources.Lang.Defense));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.Armor, hero.Stats.Armor));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.ArcaneResist, hero.Stats.ArcaneResist));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.ColdResist, hero.Stats.ColdResist));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.FireResist, hero.Stats.FireResist));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.LightningResist, hero.Stats.LightningResist));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.PhysicalResist, hero.Stats.PhysicalResist));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.PoisonResist, hero.Stats.PoisonResist));

            BattleNetData.Add(new TitleListViewData(Resources.Lang.Resources));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.PrimaryResource, hero.Stats.PrimaryResource));
            BattleNetData.Add(new ValueListViewData(Resources.Lang.SecondaryResource, hero.Stats.SecondaryResource));

            BattleNetData.Add(new TitleListViewData(Resources.Lang.Bonuses));
            BattleNetData.Add(new BonusPercentListViewData(Resources.Lang.GoldFind, hero.Stats.GoldFind));
            BattleNetData.Add(new BonusPercentListViewData(Resources.Lang.MagicFind, hero.Stats.MagicFind));
        }

        public void RefreshHero(FetchMode fetchMode = FetchMode.OnlineIfMissing)
        {
            Hero = new BindableTask<Hero>(LoadHeroAsync(fetchMode));
        }

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
