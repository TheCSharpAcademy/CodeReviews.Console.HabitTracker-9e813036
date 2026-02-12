using Microsoft.Data.Sqlite;

namespace HabitTracker.CSamu7
{
    public class DbSeeding
    {
        private readonly int _seed;
        private readonly Random _random;
        private readonly int _numHabits = 10;
        private readonly SqliteConnection _connection;
        private readonly DateTimeOffset _minDate = new DateTime(2026, 1, 1);
        private readonly DateTimeOffset _maxDate = new DateTime(2026, 2, 8);
        public DbSeeding(SqliteConnection connection, int seed) { 
            _seed = seed;
            _random = new Random(_seed);
            _connection = connection;
        }
        public void Seed()
        {
            if(IsTableEmpty("Habits")) SeedHabits();
            if(IsTableEmpty("HabitsLog")) SeedHabitsLog();
        }
        private bool IsTableEmpty(string name)
        {
            _connection.Open();

            using var command = _connection.CreateCommand();
            command.CommandText = """SELECT * FROM HabitsLog""";
            command.Parameters.AddWithValue("$name", name);

            using var reader = command.ExecuteReader();
            return !reader.HasRows;
        }
        private void SeedHabitsLog() {
            for (int i = 0; i < _numHabits * 10; i++)
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = """"
                    INSERT INTO HabitsLog(habit_id, amount, date) 
                    VALUES (@habit_id, @amount, @date)
                    """";

                command.Parameters.Add(new SqliteParameter("habit_id", (1 % 10) + 1));
                command.Parameters.Add(new SqliteParameter("amount", _random.Next(1, 8)));
                command.Parameters.Add(new SqliteParameter("date", _random.NextInt64(_minDate.ToUnixTimeSeconds(), _maxDate.ToUnixTimeSeconds())));
                command.ExecuteNonQuery();
            }
        }
        private void SeedHabits()
        {
            for(int i = 1; i <= _numHabits; i++)
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = """"INSERT INTO Habits(name) VALUES (@name)"""";
                command.Parameters.AddWithValue("name", $"Habit{i}");
                command.ExecuteNonQuery();
            }
        }

    }
}
