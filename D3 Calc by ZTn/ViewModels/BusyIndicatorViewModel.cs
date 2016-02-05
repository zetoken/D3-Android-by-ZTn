using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZTn.Pcl.D3Calculator.Annotations;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    public class BusyIndicatorViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _busyMessage;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
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
