using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Helpers;
using ZTn.Pcl.D3Calculator.Views;

namespace ZTn.Pcl.D3Calculator
{
    public class App : Application
    {
        private static readonly Task<Host[]> HostLoadTask = LoadHostAsync();

        public const string ApiKey = "zrxxcy3qzp8jcbgrce2es4yq52ew2k7r";

        private static Host[] _hosts;
        public static Host[] Hosts
        {
            get
            {
                if (_hosts == null)
                {
                    HostLoadTask.Wait();
                    _hosts = HostLoadTask.Result;
                }

                return _hosts;
            }
        }

        public App()
        {
            MainPage = new NavigationPage(new BnetAccountsPage());
        }

        public static Task<Host[]> LoadHostAsync()
        {
            return Task.Run(() =>
            {
                using (var stream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream($"{typeof(App).Namespace}.Resources.hosts.json"))
                {
                    return stream.CreateFromJsonPersistentStream<Host[]>();
                }
            });
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