using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;
using ZTn.BNet.D3.DataProviders;

[assembly: Dependency(typeof(ZTn.Windows.D3Calculator.CachedDataProvider))]

namespace ZTn.Windows.D3Calculator
{
    public class CachedDataProvider : CacheableDataProvider
    {
        #region >> Constructors

        public CachedDataProvider() :
            base(Path.Combine(ApplicationData.Current.LocalFolder.Path, "D3CalcByZTn"), new HttpRequestDataProvider())
        {
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
            Debug.WriteLine(StoragePath);
        }

        #endregion
    }
}