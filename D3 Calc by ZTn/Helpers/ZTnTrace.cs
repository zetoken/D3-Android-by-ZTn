using System;
using System.Diagnostics;
using System.Reflection;

namespace ZTnDroid.D3Calculator.Helpers
{
    public class ZTnTrace
    {
        [Conditional("DEBUG")]
        public static void Trace(MethodBase method)
        {
            Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null");
            Console.WriteLine(method.DeclaringType.Name + ": " + method.Name);
        }

        [Conditional("DEBUG")]
        public static void Trace(Object obj, MethodBase method)
        {
            Console.WriteLine(obj.GetType().Name + ": " + method.Name);
        }
    }
}