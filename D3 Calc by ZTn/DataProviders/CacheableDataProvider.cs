using System;
using System.IO;

using ZTn.BNet.D3.DataProviders;

namespace ZTnDroid.D3Calculator.DataProviders
{
    public class CacheableDataProvider : ZTn.BNet.D3.DataProviders.CacheableDataProvider
    {
        #region >> Constructors

        public CacheableDataProvider(ID3DataProvider dataProvider)
            : base(dataProvider)
        {
        }

        #endregion

        #region >> CacheableDataProvider

        /// <inheritdoc/>
        public override String GetCacheStoragePath()
        {
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "D3CalcByZTn") + "/";
        }

        #endregion
    }
}
