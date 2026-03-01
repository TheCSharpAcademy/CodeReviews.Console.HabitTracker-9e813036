using HabitTracker.CSamu7.Database;
using Microsoft.Data.Sqlite;

namespace HabitTracker.CSamu7.Habits
{
    public class HabitService
    {
        public async Task<HabitModel?> Get(int id)
        {
            using SqliteConnection connection = new SqliteConnection(DbConstants.STRING_CONNECTION);
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = "SELECT habit_id, name FROM Habits WHERE habit_id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            reader.Read();

            return reader.HasRows
                ? new HabitModel(reader.GetInt32(0), reader.GetString(1))
                : null;
        }

        public async Task<ICollection<HabitModel>> GetAll()
        {
            ICollection<HabitModel> habits = [];
            using SqliteConnection connection = new SqliteConnection(DbConstants.STRING_CONNECTION);
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = "SELECT habit_id, name FROM Habits";
            using SqliteDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                habits.Add(new HabitModel(reader.GetInt32(0), reader.GetString(1)));
            }

            return habits;
        }
        public async Task<bool> Create(string name)
        {
            using SqliteConnection connection = new SqliteConnection(DbConstants.STRING_CONNECTION);
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Habits(name) VALUES (@name)";
            command.Parameters.AddWithValue("@name", name);

            var result = await command.ExecuteNonQueryAsync();

            return result == 1;
        }

    }
}
