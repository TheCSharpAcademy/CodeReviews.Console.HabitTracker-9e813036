using Microsoft.Data.Sqlite;

namespace Kerem.HabitTracker ;


    public class DataAccess
    {
        public SqliteConnection EstablishConnection()
        {
            String connectionString = @"Data Source=C:\Users\kerem\RiderProjects\CodeReviews.Console.HabitTracker\Kerem.HabitTracker\Database\habit.db;";
            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void CreateHabitTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS habit (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    date DATETIME NOT NULL,
                    quantity INTEGER NOT NULL DEFAULT 0
            
                );
            """;
            command.ExecuteNonQuery();
            Console.WriteLine("Habit table created successfully.");
            Console.WriteLine();
        }
        
        public void DropHabitTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "DROP TABLE IF EXISTS habit;";
            command.ExecuteNonQuery();
            Console.WriteLine("Habit table dropped successfully.");
            Console.WriteLine();
        }
    }