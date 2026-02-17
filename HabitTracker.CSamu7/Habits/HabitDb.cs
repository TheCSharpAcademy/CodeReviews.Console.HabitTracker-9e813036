using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace HabitTracker.CSamu7.Habits
{
    public class HabitDb
    {
        private static readonly string STRING_CONNECTION = "Data Source=Habits.db";
        private readonly DbCreation _dbCreation = new DbCreation();
        public void CreateSchema()
        {
            SqliteConnection connection = GetConnection();

            _dbCreation.Create(connection);
        }
        public SqliteConnection GetConnection()
        {
            using SqliteConnection connection = new(STRING_CONNECTION);

            return connection;
        }
    }
}
