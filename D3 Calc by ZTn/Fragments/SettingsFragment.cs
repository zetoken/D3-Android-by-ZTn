using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZTnDroid.D3Calculator.Storage;

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
            settingOnline.Checked = D3Context.getInstance().onlineMode;
            settingOnline.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                D3Context.getInstance().onlineMode = e.IsChecked;
            };

            return view;
        }
    }
}