using System.IO;
using Xamarin.Forms;
using ZTn.Bnet.PclAdapter;
using ZTn.BNet.D3.DataProviders;

[assembly: Dependency(typeof(ZTn.Droid.D3Calculator.CachedDataProvider))]

namespace ZTn.Droid.D3Calculator
{
    public class CachedDataProvider : CacheableDataProvider
    {
        #region >> Constructors

        public CachedDataProvider() :
            base(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "D3CalcByZTn") + "/", new HttpRequestDataProvider())
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