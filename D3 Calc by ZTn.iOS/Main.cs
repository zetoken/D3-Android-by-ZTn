using UIKit;
using ZTn.Bnet.Portable.iOS;

namespace ZTn.iOS.D3Calculator
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            RegisterPcl.Register();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
