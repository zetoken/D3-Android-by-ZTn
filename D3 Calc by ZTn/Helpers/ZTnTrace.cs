using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Helpers
{
    public class ZTnTrace
    {
        public static void trace(MethodBase method)
        {
            Console.WriteLine(method.DeclaringType.Name + ": " + method.Name);
        }

        public static void trace(Object obj, MethodBase method)
        {
            Console.WriteLine(obj.GetType().Name + ": " + method.Name);
        }
    }
}