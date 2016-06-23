using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.DataProviders;
using ZTn.Pcl.D3Calculator.Annotations;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.Views;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    internal class CareersViewModel : INotifyPropertyChanged
    {
        private readonly Page _page;
        private BindableTask<Career> _career;
        public BnetAccount Account { get; }

        public ObservableCollection<IControlData> Details { get; set; }

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

        public CareersViewModel(Page page, BnetAccount account)
        {
            _page = page;
            Account = account;

            BusyIndicatorLoadingCareer = new BusyIndicatorViewModel { IsBusy = true, BusyMessage = Resources.Lang.LoadingCareer };
            Details = new ObservableCollection<IControlData>();

            RefreshCareer();
        }

        public void RefreshCareer(FetchMode fetchMode = FetchMode.OnlineIfMissing)
        {
            Career = new BindableTask<Career>(LoadCareerAsync(fetchMode));
        }

        private async Task<Career> LoadCareerAsync(FetchMode fetchMode)
        {
            BusyIndicatorLoadingCareer.IsBusy = true;

            var d3Api = App.GetD3ApiRequester(Account.Host, fetchMode);

            Career career = null;
            try
            {
                career = await d3Api.GetCareerFromBattleTagAsync(new BattleTag(Account.BattleTag));

                BuildDetails(career);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);

                await _page.DisplayAlert(Resources.Lang.ApplicationName, Resources.Lang.NetworkError, Resources.Lang.Cancel);
            }
            finally
            {
                BusyIndicatorLoadingCareer.IsBusy = false;
            }

            return career;
        }

        private void BuildDetails(Career career)
        {
            if (career == null)
            {
                return;
            }

            Details.Clear();

            Details.Add(new TitleData(Resources.Lang.Career));
            Details.Add(new TextData(Resources.Lang.GuildName, career.GuildName));
            Details.Add(new ValueData(Resources.Lang.Elites, career.Kills.Elites));
            Details.Add(new ValueData(Resources.Lang.Monsters, career.Kills.Monsters));
            Details.Add(new ValueData(Resources.Lang.HardcoreMonsters, career.Kills.HardcoreMonsters));
            Details.Add(new ValueData(Resources.Lang.ParagonLevel, career.ParagonLevel));
            Details.Add(new ValueData(Resources.Lang.ParagonLevelHardcore, career.ParagonLevelHardcore));
            Details.Add(new ValueData(Resources.Lang.ParagonLevelSeason, career.ParagonLevelSeason));
            Details.Add(new ValueData(Resources.Lang.ParagonLevelSeasonHardcore, career.ParagonLevelSeasonHardcore));

            Details.Add(new TitleData(Resources.Lang.Heroes));
            foreach (var hero in career.Heroes)
            {
                Details.Add(new HeroData(hero));
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
