using SQLite;

namespace ContactBook.Persistence
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

