using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Helpers
{
    /// <summary>
    /// Wrapper of any .net <see cref="Object"/> into a Xamarin <see cref="Java.Lang.Object"/> for use with Android functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JavaLangObject<T> : Java.Lang.Object
    {
        T _value;

        public T value
        {
            get { return _value; }
        }

        public JavaLangObject(T value)
        {
            this._value = value;
        }
    }
}