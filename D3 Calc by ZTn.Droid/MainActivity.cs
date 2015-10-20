using Android.App;
using Android.Content.PM;
using Android.OS;
using ZTn.Bnet.Portable.Android;
using ZTn.Pcl.D3Calculator;

namespace ZTn.Droid.D3Calculator
{
    [Activity(Label = "D3 Calc by ZTn", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation /*, Theme = "@android:style/Theme.Material.Light"*/)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public MainActivity()
        {
            RegisterPcl.Register();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

