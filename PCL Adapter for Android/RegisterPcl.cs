namespace ZTn.Bnet.Portable.Android
{
    public class RegisterPcl
    {
        private static bool _registeringDone = false;

        public static void Register()
        {
            if (_registeringDone)
            {
                return;
            }

            PortableInjector.Register<IPortableDirectory>(new PortableDirectory());
            PortableInjector.Register<IPortableEncoding>(new PortableEncoding());
            PortableInjector.Register<IPortableFile>(new PortableFile());

            _registeringDone = true;
        }
    }
}
