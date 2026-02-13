using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
