namespace ZTn.Bnet.Portable.Android
{
    public class RegisterPcl
    {
        public static void Register()
        {
            PortableInjector.Register<IPortableDirectory>(new Android.PortableDirectory());
            PortableInjector.Register<IPortableEncoding>(new Android.PortableEncoding());
            PortableInjector.Register<IPortableFile>(new Android.PortableFile());
        }
    }
}
