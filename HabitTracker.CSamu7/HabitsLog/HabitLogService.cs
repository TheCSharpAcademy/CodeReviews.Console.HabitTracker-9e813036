using HabitTracker.CSamu7.Habits;
using Microsoft.Data.Sqlite;

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
    }
}
