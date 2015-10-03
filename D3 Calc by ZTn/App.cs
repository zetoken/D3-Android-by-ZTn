using System.Reflection;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Helpers;
using ZTn.Pcl.D3Calculator.Views;

namespace ZTn.Pcl.D3Calculator
{
    public class App : Application
    {
        public App()
        {
            MainPage = new MasterPage();
        }

        private static Host[] _hosts;

        public static Host[] Hosts
        {
            get
            {
                if (_hosts == null)
                {
                    using (var stream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream($"{typeof(App).Namespace}.Resources.hosts.json"))
                    {
                        _hosts = stream.CreateFromJsonPersistentStream<Host[]>();
                    }
                }

                return _hosts;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}