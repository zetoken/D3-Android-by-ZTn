using System.IO;
using SQLite;
using Xamarin.Forms;
using ZTn.Droid.D3Calculator;
using ZTn.Pcl.D3Calculator;

[assembly: Dependency(typeof(SqLiteForAndroid))]

namespace ZTn.Droid.D3Calculator
{
    /// <summary>
    /// Android implementation of <see cref="ISqLite"/>.
    /// </summary>
    public class SqLiteForAndroid : ISqLite
    {
        /// <inheritdoc />
        public SQLiteConnection GetConnection()
        {
            const string sqliteFilename = "d3calculator.db";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}