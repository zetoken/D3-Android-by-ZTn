using System.IO;

namespace ZTn.Bnet.Portable.Android
{
    internal class PortableDirectory : IPortableDirectory
    {
        /// <inheritdoc />
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}