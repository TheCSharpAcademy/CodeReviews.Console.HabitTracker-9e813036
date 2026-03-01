using HabitTracker.CSamu7.Database;
using Microsoft.Data.Sqlite;

namespace HabitTracker.CSamu7.HabitsLog
{
    public class HabitLogService
    {
        public async Task<HabitLogModel?> Get(int idHabit)
        {
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM HabitsLog WHERE habit_log_id = @id";
            command.Parameters.AddWithValue("id", idHabit);

            using var reader = await command.ExecuteReaderAsync();
            reader.Read();

            return reader.HasRows
                ? new(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt64(3))
                : null;
        }
        public async Task<ICollection<HabitLogModel>> GetAll()
        {
            ICollection<HabitLogModel> habits = [];
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = """
                SELECT H1.habit_log_id, H.name, H1.amount, H1.date FROM HabitsLog as H1 
                INNER JOIN Habits as H ON H1.habit_id = H.habit_id 
                ORDER BY H1.date ASC
                """;

            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                int idHabitLog = reader.GetInt32(0);
                string habitName = reader.GetString(1);
                int amount = reader.GetInt32(2);
                long timestamp = reader.GetInt64(3);

                habits.Add(new(idHabitLog, habitName, amount, timestamp));
            }

            return habits;
        }
        public async Task<bool> Create(PostHabitLogModel model)
        {
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO HabitsLog(habit_id, amount, date) VALUES (@habit_id, @amount, @date)";
            command.Parameters.AddWithValue("@habit_id", model.IdHabit);
            command.Parameters.AddWithValue("@amount", model.Amount);
            command.Parameters.AddWithValue("@date", model.UnixTimestamp);

            var result = await command.ExecuteNonQueryAsync();

            return result == 1;
        }
        public async Task<bool> Modify(HabitLogModel newHabit)
        {
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = """
                UPDATE HabitsLog
                SET amount = @amount, date = @date
                WHERE habit_log_id = @habitLogId
                """;
            command.Parameters.AddWithValue("@amount", newHabit.Amount);
            command.Parameters.AddWithValue("@date", newHabit.UnixTimeStamp);
            command.Parameters.AddWithValue("@habitLogId", newHabit.Id);

            var result = await command.ExecuteNonQueryAsync();

            return result == 1;
        }
        public async Task<bool> Delete(int idHabitLog)
        {
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM HabitsLog WHERE habit_log_id = @habit_log_id";

            command.Parameters.Add(new("@habit_log_id", idHabitLog));

            var result = await command.ExecuteNonQueryAsync();

            return result == 1;
        }
    }
}
