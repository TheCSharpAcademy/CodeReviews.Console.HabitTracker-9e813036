using Microsoft.Data.Sqlite;

namespace HabitTracker.CSamu7.Database
{
    public class DbSeeding
    {
        private readonly Random _random;
        private readonly int _numHabits = 10;
        private readonly DateTimeOffset _minDate = new DateTime(2026, 1, 1);
        private readonly DateTimeOffset _maxDate = new DateTime(2026, 2, 8);
        public DbSeeding(int seed = 1)
        {
            _random = new Random(seed);
        }
        public async Task Seed()
        {
            bool isSeededHabits = await IsTableEmpty("Habits");
            bool isSeededHabitsLog = await IsTableEmpty("HabitsLog");

            if (isSeededHabits) SeedHabits();
            if (isSeededHabitsLog) SeedHabitsLog();
        }
        private async Task<bool> IsTableEmpty(string name)
        {
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {name}";

            using var reader = await command.ExecuteReaderAsync();
            return !reader.HasRows;
        }
        private void SeedHabits()
        {
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            for (int i = 1; i <= _numHabits; i++)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = """"INSERT INTO Habits(name) VALUES (@name)"""";
                command.Parameters.AddWithValue("name", $"Habit{i}");
                command.ExecuteNonQuery();
            }
        }
        private void SeedHabitsLog()
        {
            using SqliteConnection connection = new(DbConstants.STRING_CONNECTION);
            connection.Open();

            for (int i = 0; i < _numHabits * 10; i++)
            {
                var command = connection.CreateCommand();
                command.CommandText = """"
                    INSERT INTO HabitsLog(habit_id, amount, date) 
                    VALUES (@habit_id, @amount, @date)
                    """";

                command.Parameters.AddWithValue("habit_id", i % 10 + 1);
                command.Parameters.AddWithValue("amount", _random.Next(1, 8));
                command.Parameters.AddWithValue("date", _random.NextInt64(_minDate.ToUnixTimeSeconds()));

                command.ExecuteNonQuery();
            }
        }
    }
}
