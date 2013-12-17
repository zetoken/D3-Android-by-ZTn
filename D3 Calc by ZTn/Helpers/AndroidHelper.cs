using System;
using System.Diagnostics;

namespace ZTnDroid.D3Calculator.Helpers
{
    /// <summary>
    /// Container for some common Android extension helpers
    /// </summary>
    public static class AndroidHelper
    {
        /// <summary>
        /// Returns a String representing the canonical name of a type as needed for some Android methods.
        /// - namespace is lower case (as a java package name should be)
        /// - class name is as is (as a java class name)
        /// </summary>
        /// <param name="type"><seealso cref="Type"/> of the .net class</param>
        /// <returns></returns>
        public static String ToAndroidClassName(this Type type)
        {
            Debug.Assert(type.Namespace != null, "type.Namespace != null");
            return type.Namespace.ToLower() + "." + type.Name;
        }
    }
}