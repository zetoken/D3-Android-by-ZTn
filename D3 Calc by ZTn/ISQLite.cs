using SQLite;

namespace ZTn.Pcl.D3Calculator
{
    public interface ISqLite
    {
        SQLiteConnection GetConnection();
    }
}
