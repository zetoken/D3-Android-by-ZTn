using Android.OS;
using Android.Views;
using Android.Widget;
using System.Reflection;
using ZTn.BNet.D3.DataProviders;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class SettingsFragment : Fragment
    {
        #region >> Fragment

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());
            base.OnCreate(savedInstanceState);
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());
            View view = inflater.Inflate(Resource.Layout.Settings, container, false);

            Activity.Title = Resources.GetString(Resource.String.Settings);

            var settingOnline = view.FindViewById<Switch>(Resource.Id.settingOnline);
            settingOnline.Checked = (D3Context.Instance.FetchMode == OnlineMode.Online);
            settingOnline.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                D3Calc.Preferences
                    .Edit()
                    .PutBoolean(D3Calc.SettingsOnlinemode, e.IsChecked)
                    .Commit();
                D3Context.Instance.FetchMode = (e.IsChecked ? OnlineMode.Online : OnlineMode.Offline);
            };

            return view;
        }

        #endregion
    }
}