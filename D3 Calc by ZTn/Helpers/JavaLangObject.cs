using System;

namespace ZTnDroid.D3Calculator.Helpers
{
    /// <summary>
    /// Wrapper of any .net <see cref="Object"/> into a Xamarin <see cref="Java.Lang.Object"/> for use with Android functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JavaLangObject<T> : Java.Lang.Object
    {
        readonly T value;

        public T Value
        {
            get { return value; }
        }

        public JavaLangObject(T value)
        {
            this.value = value;
        }
    }
}