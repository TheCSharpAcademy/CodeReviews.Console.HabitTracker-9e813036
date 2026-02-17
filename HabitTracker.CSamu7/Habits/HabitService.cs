using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace HabitTracker.CSamu7.Habits
{
    public class HabitService
    {
        private readonly HabitDb db = new HabitDb();
        public async Task<bool> PostHabit(string name)
        {
            SqliteConnection connection = db.GetConnection();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Habits(name) VALUES (@name)";
            command.Parameters.AddWithValue("@name", name);

            var result = await command.ExecuteNonQueryAsync();

            return result == 1;
        }
        public async Task<List<HabitModel>> GetHabits()
        {
            List<HabitModel> habits = [];
            var connection = db.GetConnection();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = "SELECT habit_id, name FROM Habits";
            SqliteDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                habits.Add(new HabitModel(reader.GetInt32(0), reader.GetString(1)));
            }

            return habits;
        }
    }
}
