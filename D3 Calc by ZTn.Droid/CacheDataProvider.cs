using System.IO;
using Xamarin.Forms;
using ZTn.Bnet.Portable;
using ZTn.BNet.D3.DataProviders;

[assembly: Dependency(typeof(ZTn.Droid.D3Calculator.CacheableDataProvider))]

namespace ZTn.Droid.D3Calculator
{
    public class CacheableDataProvider : BNet.D3.DataProviders.CacheableDataProvider
    {
        #region >> Constructors

        public CacheableDataProvider() : base(new HttpRequestDataProvider())
        {
            var noMediaFile = GetCacheStoragePath() + ".nomedia";
            if (!File.Exists(noMediaFile))
            {
                var directoryName = Path.GetDirectoryName(noMediaFile);
                if (!string.IsNullOrWhiteSpace(directoryName))
                {
                    PortableDirectory.CreateDirectory(directoryName);
                }
                File.Create(noMediaFile);
            }
        }

        #endregion

        #region >> CacheableDataProvider

        /// <inheritdoc/>
        public override sealed string GetCacheStoragePath()
        {
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "D3CalcByZTn") + "/";
        }

        #endregion
    }
}