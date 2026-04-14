using HabitTracker.Model;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.Data
{
    public class DatabaseManager(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        private T ExecuteCommand<T>(string commandText, Dictionary<string, object>? parameters, Func<SqliteCommand, T> executeAction)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = commandText;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            return executeAction(command);
        }

        private void RunNonQuery(string commandText, Dictionary<string, object>? parameters = null) =>
            ExecuteCommand(commandText, parameters, command => command.ExecuteNonQuery());

        private int RunScalar(string commandText, Dictionary<string, object>? parameters = null) =>
            ExecuteCommand(commandText, parameters, command => Convert.ToInt32(command.ExecuteScalar()));

        public void CreateTable()
        {
            RunNonQuery("""
            CREATE TABLE IF NOT EXISTS habit_records (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                HabitName TEXT,
                Unit TEXT,
                Date TEXT,
                Quantity INTEGER
            )
            """);
        }

        public void SeedData()
        {
            int recordCount = RunScalar("SELECT COUNT(*) FROM habit_records");
            if (recordCount > 0) return;

            var predefinedHabits = new[]
            {
            new { Name = "Running", Unit = "km" },
            new { Name = "Drinking Water", Unit = "glasses" },
            new { Name = "Reading", Unit = "pages" }
        };

            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var randomHabit = predefinedHabits[random.Next(predefinedHabits.Length)];

                InsertRecord(new HabitRecord
                {
                    HabitName = randomHabit.Name,
                    Unit = randomHabit.Unit,
                    Date = DateTime.Now.AddDays(-random.Next(0, 365)),
                    Quantity = random.Next(1, 20)
                });
            }
        }

        public List<HabitRecord> GetAllRecords()
        {
            return ExecuteCommand("SELECT * FROM habit_records", null, command =>
            {
                List<HabitRecord> records = new();
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        records.Add(new HabitRecord
                        {
                            Id = reader.GetInt32(0),
                            HabitName = reader.GetString(1),
                            Unit = reader.GetString(2),
                            Date = DateTime.ParseExact(reader.GetString(3), "dd-MM-yyyy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(4)
                        });
                    }
                }
                return records;
            });
        }

        public void InsertRecord(HabitRecord record)
        {
            RunNonQuery(
                "INSERT INTO habit_records(HabitName, Unit, Date, Quantity) VALUES(@name, @unit, @date, @quantity)",
                new Dictionary<string, object> { { "@name", record.HabitName }, { "@unit", record.Unit }, { "@date", record.Date.ToString("dd-MM-yyyy") }, { "@quantity", record.Quantity } });
        }

        public bool DeleteRecord(int id)
        {
            return ExecuteCommand(
                "DELETE from habit_records WHERE Id = @id",
                new Dictionary<string, object> { { "@id", id } },
                command => command.ExecuteNonQuery()) > 0;
        }

        public bool CheckRecordExists(int id)
        {
            return RunScalar(
                "SELECT EXISTS(SELECT 1 FROM habit_records WHERE Id = @id)",
                new Dictionary<string, object> { { "@id", id } }) != 0;
        }

        public void UpdateRecord(HabitRecord record)
        {
            RunNonQuery(
                "UPDATE habit_records SET HabitName = @name, Unit = @unit, Date = @date, Quantity = @quantity WHERE Id = @id",
                new Dictionary<string, object> { { "@name", record.HabitName }, { "@unit", record.Unit }, { "@date", record.Date.ToString("dd-MM-yyyy") }, { "@quantity", record.Quantity }, { "@id", record.Id } });
        }
    }
}
