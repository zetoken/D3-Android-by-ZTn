using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using ZTn.iOS.D3Calculator;
using ZTn.Pcl.D3Calculator;

[assembly: Dependency(typeof(SqliteForIos))]

namespace ZTn.iOS.D3Calculator
{
    internal class SqliteForIos : ISqLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "d3calculator.db";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}
