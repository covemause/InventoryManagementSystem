using System.Data.SQLite;

namespace InventoryManagementSystem.DataBase
{
    public class Database
    {
        private const string DatabaseFile = "customer.db";
        private const string ConnectionString = "Data Source=" + DatabaseFile + ";Version=3;";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }
    }
} 