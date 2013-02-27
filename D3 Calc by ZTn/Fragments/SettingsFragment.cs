using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.DataProviders;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class SettingsFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("SettingsFragment: OnCreate");
            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("SettingsFragment: OnCreate");
            View view = inflater.Inflate(Resource.Layout.Settings, container, false);

            Activity.Title = Resources.GetString(Resource.String.Settings);

            Switch settingOnline = view.FindViewById<Switch>(Resource.Id.settingOnline);
            settingOnline.Checked = (D3Context.getInstance().onlineMode == OnlineMode.Online);
            settingOnline.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                HomeActivity.preferences
                    .Edit()
                    .PutBoolean(HomeActivity.SETTINGS_ONLINEMODE, e.IsChecked)
                    .Commit();
                D3Context.getInstance().onlineMode = (e.IsChecked ? OnlineMode.Online : OnlineMode.Offline);
            };

            return view;
        }
    }
}