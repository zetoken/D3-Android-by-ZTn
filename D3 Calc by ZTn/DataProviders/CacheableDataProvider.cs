using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Android.Database;
using ZTn.BNet.D3.DataProviders;

namespace ZTnDroid.D3Calculator.DataProviders
{
    public class CacheableDataProvider : ZTn.BNet.D3.DataProviders.CacheableDataProvider
    {
        #region >> Fields

        Android.Content.Context context;
        static Storage.CacheDB db;

        #endregion

        #region >> Constructors

        public CacheableDataProvider(Android.Content.Context context, ID3DataProvider dataProvider)
            : base(dataProvider)
        {
            this.context = context;
            db = new Storage.CacheDB(context);
        }

        #endregion

        public override String getCacheStoragePath()
        {
            return context.FilesDir + "/cache/";
        }
    }
}
