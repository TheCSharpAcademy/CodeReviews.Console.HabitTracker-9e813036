using HabitTracker.CSamu7.Habits;
using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Threading.Tasks;

namespace HabitTracker.CSamu7.HabitsLog
{
    public class HabitLogService
    {
        private readonly HabitDb db = new HabitDb();
        public bool CreateHabitLog(PostHabitLogModel model)
        {
            SqliteConnection connection = db.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO HabitsLog(habit_id, amount, date) VALUES (@habit_id, @amount, @date)";
            
            command.Parameters.Add(new SqliteParameter("@habit_id", model.IdHabit));
            command.Parameters.Add(new SqliteParameter("@amount", model.Amount));
            command.Parameters.Add(new SqliteParameter("@date", model.UnixTimestamp));

            var result = command.ExecuteNonQuery();

            return result == 1;
        }
        public async Task<ICollection<HabitLogModel>> GetHabitLogs()
        {
            ICollection<HabitLogModel> model = [];
            SqliteConnection connection = db.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                SELECT H1.habit_log_id, H.name, H1.amount, H1.date FROM HabitsLog as H1 
                INNER JOIN Habits as H ON H1.habit_id = H.habit_id 
                ORDER BY H1.date ASC
                """;

            var result = await command.ExecuteReaderAsync();

            while (result.Read()) {
                int idHabitLog = result.GetInt32(0);
                string habitName = result.GetString(1);
                int amount = result.GetInt32(2);
                long timestamp = result.GetInt64(3);

                model.Add(new(idHabitLog, habitName, amount, timestamp));  
            }

            return model;
        }
    }
}
