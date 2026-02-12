using Microsoft.Data.Sqlite;
//TODO: Refactorizar el codigo de seeding
namespace HabitTracker.CSamu7
{
    public class DbCreation
    {
        public void Create(SqliteConnection connection)
        {
            DbSeeding seeding = new DbSeeding(connection, 10);

            var command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS Habits 
                (habit_id INTEGER PRIMARY KEY, name TEXT NOT NULL)
                """;

            connection.Open();
            command.ExecuteNonQuery();

            command.CommandText = """
                CREATE TABLE IF NOT EXISTS HabitsLog
                (
                    habit_log_id INTEGER PRIMARY KEY, 
                    habit_id INTEGER NOT NULL, 
                    amount INTEGER NOT NULL,
                    date INTEGER NOT NULL,
                    FOREIGN KEY (habit_id) REFERENCES habits(habit_id) ON DELETE CASCADE ON DELETE NO ACTION
                )
                """;

            command.ExecuteNonQuery();
            seeding.Seed();
        }
    }
}
