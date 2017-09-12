using System;
using System.IO;
using ZTn.Bnet.PclAdapter;
using ZTn.BNet.D3.DataProviders;

namespace ZTnDroid.D3Calculator.DataProviders
{
    public class CacheableDataProvider : ZTn.BNet.D3.DataProviders.CacheableDataProvider
    {
        #region >> Constructors

        public CacheableDataProvider(ID3DataProvider dataProvider)
            : base(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "D3CalcByZTn") + "/", dataProvider)
        {
            var noMediaFile = StoragePath + ".nomedia";
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
    }
}
