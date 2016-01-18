using System.IO;
using Windows.Storage;
using SQLite;
using Xamarin.Forms;
using ZTn.Pcl.D3Calculator;
using ZTn.Windows.D3Calculator;

[assembly: Dependency(typeof(SqLiteForUniversalWindows))]

namespace ZTn.Windows.D3Calculator
{
    /// <summary>
    /// Android implementation of <see cref="ISqLite"/>.
    /// </summary>
    public class SqLiteForUniversalWindows : ISqLite
    {
        /// <inheritdoc />
        public SQLiteConnection GetConnection()
        {
            const string sqliteFilename = "d3calculator.db";
            var localPath = ApplicationData.Current.LocalFolder.Path;
            var path = Path.Combine(localPath, sqliteFilename);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}