using System;
using System.IO;

using ZTn.BNet.D3.DataProviders;
using ZTn.Bnet.Portable;

namespace ZTnDroid.D3Calculator.DataProviders
{
    public class CacheableDataProvider : ZTn.BNet.D3.DataProviders.CacheableDataProvider
    {
        #region >> Constructors

        public CacheableDataProvider(ID3DataProvider dataProvider)
            : base(dataProvider)
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
        public override sealed String GetCacheStoragePath()
        {
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "D3CalcByZTn") + "/";
        }

        #endregion
    }
}
