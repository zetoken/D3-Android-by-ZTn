using System.Text;

namespace ZTn.Bnet.Portable.Android
{
    internal class PortableEncoding : IPortableEncoding
    {
        /// <inheritdoc />
        public Encoding Default
        {
            get { return Encoding.Default; }
        }
    }
}