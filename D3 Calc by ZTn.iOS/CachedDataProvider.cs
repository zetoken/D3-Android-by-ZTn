using System;
using System.IO;
using Xamarin.Forms;
using ZTn.Bnet.PclAdapter;
using ZTn.BNet.D3.DataProviders;

[assembly: Dependency(typeof(ZTn.iOS.D3Calculator.CachedDataProvider))]

namespace ZTn.iOS.D3Calculator
{
    public class CachedDataProvider : CacheableDataProvider
    {
        #region >> Constructors

        public CachedDataProvider() :
            base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "D3CalcByZTn") + "/", new HttpRequestDataProvider())
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