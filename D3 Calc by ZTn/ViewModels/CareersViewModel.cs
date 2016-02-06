using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.DataProviders;
using ZTn.Pcl.D3Calculator.Annotations;
using ZTn.Pcl.D3Calculator.Models;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    internal class CareersViewModel : INotifyPropertyChanged
    {
        private BindableTask<Career> _career;
        public BnetAccount Account { get; }

        public ObservableCollection<IListViewRowData> Details { get; set; }

        public BindableTask<Career> Career
        {
            get { return _career; }
            set
            {
                _career = value;
                OnPropertyChanged();
            }
        }

        public BusyIndicatorViewModel BusyIndicatorLoadingCareer { get; }

        public CareersViewModel(BnetAccount account)
        {
            Account = account;

            BusyIndicatorLoadingCareer = new BusyIndicatorViewModel { IsBusy = true, BusyMessage = Resources.Lang.LoadingCareer };
            Details = new ObservableCollection<IListViewRowData>();

            RefreshCareer();
        }

        public void RefreshCareer(FetchMode fetchMode = FetchMode.OnlineIfMissing)
        {
            Career = new BindableTask<Career>(LoadCareerAsync(fetchMode));
        }

        private async Task<Career> LoadCareerAsync(FetchMode fetchMode)
        {
            BusyIndicatorLoadingCareer.IsBusy = true;

            var dataProvider = DependencyService.Get<ICacheableD3DataProvider>();
            dataProvider.FetchMode = fetchMode;

            var d3Api = new D3ApiRequester
            {
                ApiKey = App.ApiKey,
                DataProvider = dataProvider,
                Host = Account.Host
            };

            var career = await d3Api.GetCareerFromBattleTagAsync(new BattleTag(Account.BattleTag));

            BusyIndicatorLoadingCareer.IsBusy = false;

            BuildDetails(career);

            return career;
        }

        private void BuildDetails(Career career)
        {
            if (career == null)
            {
                return;
            }

            Details.Clear();

            Details.Add(new TitleListViewData(Resources.Lang.Career));
            Details.Add(new TextListViewData(Resources.Lang.GuildName, career.GuildName));
            Details.Add(new ValueListViewData(Resources.Lang.Elites, career.Kills.Elites));
            Details.Add(new ValueListViewData(Resources.Lang.Monsters, career.Kills.Monsters));
            Details.Add(new ValueListViewData(Resources.Lang.HardcoreMonsters, career.Kills.HardcoreMonsters));
            Details.Add(new ValueListViewData(Resources.Lang.ParagonLevel, career.ParagonLevel));
            Details.Add(new ValueListViewData(Resources.Lang.ParagonLevelHardcore, career.ParagonLevelHardcore));
            Details.Add(new ValueListViewData(Resources.Lang.ParagonLevelSeason, career.ParagonLevelSeason));
            Details.Add(new ValueListViewData(Resources.Lang.ParagonLevelSeasonHardcore, career.ParagonLevelSeasonHardcore));

            Details.Add(new TitleListViewData(Resources.Lang.Heroes));
            foreach (var hero in career.Heroes)
            {
                Details.Add(new HeroListViewData(hero));
            }
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
