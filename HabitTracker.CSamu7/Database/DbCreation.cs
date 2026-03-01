using Microsoft.Data.Sqlite;

namespace HabitTracker.CSamu7.Database
{
    public class DbCreation
    {
        private readonly string _queryCreateHabitsTable = """
                CREATE TABLE IF NOT EXISTS Habits 
                (habit_id INTEGER PRIMARY KEY, name TEXT NOT NULL)
                """;
        private readonly string _queryCreateHabitsLogTable = """
                CREATE TABLE IF NOT EXISTS HabitsLog
                (
                    habit_log_id INTEGER PRIMARY KEY, 
                    habit_id INTEGER NOT NULL, 
                    amount INTEGER NOT NULL,
                    date INTEGER NOT NULL,
                    FOREIGN KEY (habit_id) REFERENCES habits(habit_id) ON DELETE CASCADE ON DELETE NO ACTION
                )
                """;
        public async Task Create()
        {
            using SqliteConnection connection = new SqliteConnection(DbConstants.STRING_CONNECTION);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = _queryCreateHabitsTable;
            command.ExecuteNonQuery();

            command.CommandText = _queryCreateHabitsLogTable;
            command.ExecuteNonQuery();

            DbSeeding seeding = new DbSeeding();
            await seeding.Seed();
        }
    }
}
