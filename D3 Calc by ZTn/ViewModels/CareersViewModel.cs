using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Annotations;
using ZTn.Pcl.D3Calculator.Models;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    class CareersViewModel : INotifyPropertyChanged
    {
        private readonly BnetAccount _account;

        public event PropertyChangedEventHandler PropertyChanged;

        public Career Career { get; private set; }
        public HeroSummary[] Heroes { get; private set; }

        public CareersViewModel(BnetAccount account)
        {
            _account = account;

            LoadCareerAsync();
        }

        private void LoadCareerAsync()
        {
            Task.Run(() =>
            {
                D3Api.Host = _account.Host;

                Career = Career.CreateFromBattleTag(new BattleTag(_account.BattleTag));
                OnPropertyChanged(nameof(Career));

                Heroes = Career?.Heroes;
                OnPropertyChanged(nameof(Heroes));

                return Career;
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
