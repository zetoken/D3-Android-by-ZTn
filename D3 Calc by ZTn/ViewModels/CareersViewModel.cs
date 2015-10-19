using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Annotations;
using ZTn.Pcl.D3Calculator.Models;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    class CareersViewModel : INotifyPropertyChanged
    {
        private readonly BnetAccount _account;
        private bool _isBusy;
        private string _busyMessage;
        private Career _career;
        private HeroSummary[] _heroes;

        public Career Career
        {
            get { return _career; }
            private set
            {
                _career = value;
                OnPropertyChanged();
            }
        }

        public HeroSummary[] Heroes
        {
            get { return _heroes; }
            private set
            {
                _heroes = value;
                OnPropertyChanged();
            }
        }

        public string CareerTitle => Resources.Lang.Career.ToUpper();
        public string HeroesTitle => Resources.Lang.Heroes.ToUpper();

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                BusyIndicatorLoadingCareer.IsBusy = value;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string BusyMessage
        {
            get { return _busyMessage; }
            set
            {
                _busyMessage = value;
                OnPropertyChanged();
            }
        }

        public BusyIndicatorViewModel BusyIndicatorLoadingCareer { get; set; }

        public CareersViewModel(BnetAccount account)
        {
            _account = account;
            BusyIndicatorLoadingCareer = new BusyIndicatorViewModel { IsBusy = true, BusyMessage = "Loading Career" };

            LoadCareerAsync();
        }

        public Task<Career> LoadCareerAsync(FetchMode fetchMode = FetchMode.OnlineIfMissing)
        {
            return Task.Run(() =>
            {
                BusyMessage = "Loading career";
                IsBusy = true;

                var dataProvider = DependencyService.Get<CacheableDataProvider>();
                dataProvider.FetchMode = fetchMode;

                var d3Api = new D3ApiRequester
                {
                    ApiKey = App.ApiKey,
                    DataProvider = dataProvider,
                    Host = _account.Host
                };

                Career = d3Api.GetCareerFromBattleTag(new BattleTag(_account.BattleTag));

                Heroes = Career?.Heroes;

                IsBusy = false;

                return Career;
            });
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
