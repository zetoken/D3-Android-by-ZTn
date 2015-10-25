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

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    class HeroViewModel : INotifyPropertyChanged
    {
        private readonly BnetAccount _account;
        private BindableTask<Hero> _hero;
        private readonly HeroSummary _heroSummary;

        public BindableTask<Hero> Hero
        {
            get { return _hero; }
            private set
            {
                _hero = value;
                OnPropertyChanged();
            }
        }

        public string HeroTitle => Resources.Lang.Hero.ToUpper();

        public BusyIndicatorViewModel BusyIndicatorLoadingCareer { get; }

        public HeroViewModel(BnetAccount account, HeroSummary heroSummary)
        {
            _account = account;
            _heroSummary = heroSummary;

            BusyIndicatorLoadingCareer = new BusyIndicatorViewModel { IsBusy = true, BusyMessage = Resources.Lang.LoadingHero };

            RefreshHero();
        }

        private async Task<Hero> LoadHeroAsync(FetchMode fetchMode)
        {
            BusyIndicatorLoadingCareer.IsBusy = true;

            var dataProvider = DependencyService.Get<CacheableDataProvider>();
            dataProvider.FetchMode = fetchMode;

            var d3Api = new D3ApiRequester
            {
                ApiKey = App.ApiKey,
                DataProvider = dataProvider,
                Host = _account.Host
            };

            var hero = await d3Api.GetHeroFromHeroIdAsync(new BattleTag(_account.BattleTag), _heroSummary.Id);

            BusyIndicatorLoadingCareer.IsBusy = false;

            return hero;
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
