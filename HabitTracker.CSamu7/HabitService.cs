using Microsoft.Data.Sqlite;

namespace HabitTracker.CSamu7
{
    public class HabitService
    {
        private readonly HabitDb db = new HabitDb();
        public bool PostHabit(string name)
        {
            var connection = db.GetConnection();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Habits(name) VALUES (@name)";
            command.Parameters.AddWithValue("@name", name);

            var result = command.ExecuteNonQuery();

            return result == 1;
        }
        public List<Habit> GetHabits()
        {
            List<Habit> habits = [];
            var connection = db.GetConnection();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = "SELECT habit_id, name FROM Habits";
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                habits.Add(new Habit(reader.GetInt32(0), reader.GetString(1)));
            }

            return habits;
        }
    }
}
