using System.Diagnostics;
using Xamarin.Forms;

namespace ZTn.Pcl.D3Calculator.Controls
{
    public partial class BusyIndicator : View
    {
        public static BindableProperty BusyMessageBindableProperty =
            BindableProperty.Create<BusyIndicator, string>(x => x.BusyMessage, default(string), BindingMode.OneWay, null, OnBusyMessageChanged);

        public string BusyMessage
        {
            get { return (string)GetValue(BusyMessageBindableProperty); }
            set { SetValue(BusyMessageBindableProperty, value); }
        }

        static void OnBusyMessageChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var control = (BusyIndicator)bindable;
            control.Message.Text = newValue;
        }

        public static BindableProperty IsBusyBindableProperty =
            BindableProperty.Create<BusyIndicator, bool>(x => x.IsBusy, true, BindingMode.OneWay, null, OnIsBusyChanged);

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyBindableProperty); }
            set { SetValue(IsBusyBindableProperty, value); }
        }

        static void OnIsBusyChanged(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            Debug.WriteLine("OnIsBusy");
            var control = (BusyIndicator)bindable;
            control.Indicator.IsRunning = newvalue;
            control.Stack.IsVisible = newvalue;
        }

        public BusyIndicator()
        {
            InitializeComponent();

            IsBusy = true;

            BindingContext = this;
        }
    }
}
