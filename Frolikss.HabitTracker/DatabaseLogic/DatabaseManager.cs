using System.Globalization;
using Microsoft.Data.Sqlite;

namespace DatabaseLogic;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Units { get; set; }
}

public class Data
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public DateTime? Date { get; set; }
    public Habit Habit { get; set; }

    public Data()
    {
        Habit = new Habit();
    }
}

public class DatabaseManager
{
    private static string dateFormat = "yyyy-MM-dd";
    private string connectionString;
    private string habitTypesTableName = "habit_types";
    private string habitsTableName = "habit_logger";

    public DatabaseManager(string connectionString)
    {
        this.connectionString = connectionString;

        ExecuteQuery((command) =>
        {
            command.CommandText = (@$"
                CREATE TABLE IF NOT EXISTS {habitTypesTableName}
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Units TEXT
                );
                CREATE TABLE IF NOT EXISTS {habitsTableName}
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitTypeId INTEGER,
                    Quantity INTEGER,
                    Date TEXT,
                    FOREIGN KEY (HabitTypeId) REFERENCES {habitTypesTableName}(Id)
                );");
        });
        SeedHabitsTypes();
        SeedHabitLogs();
    }

    void ExecuteQuery(Action<SqliteCommand>? configure = null)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using var pragma = connection.CreateCommand();
                pragma.CommandText = @"PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();

                var command = connection.CreateCommand();
                configure?.Invoke(command);

                command.ExecuteNonQuery();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine($"Error occured: {err.Message}");
        }
    }
    
    void ExecuteReader(Action<SqliteCommand> configure)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var pragma = connection.CreateCommand();
        pragma.CommandText = "PRAGMA foreign_keys = ON;";
        pragma.ExecuteNonQuery();

        using var command = connection.CreateCommand();
        configure(command);
    }
    
    T ExecuteScalar<T>(Func<SqliteCommand, T> configure)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var pragma = connection.CreateCommand();
        pragma.CommandText = "PRAGMA foreign_keys = ON;";
        pragma.ExecuteNonQuery();

        using var command = connection.CreateCommand();
        return configure(command);
    }

    public int? AddHabit(Habit habit)
    {
        int? habitId = null;
        var commandText = $@"
            INSERT INTO {habitTypesTableName} (Name, Units)
            VALUES (@name, @units);
            SELECT last_insert_rowid();
        ";

        habitId = ExecuteScalar((command) =>
        {
            command.CommandText = commandText;

            command.Parameters.AddWithValue("@name", habit.Name);
            command.Parameters.AddWithValue("@units", habit.Units);

            return Convert.ToInt32(command.ExecuteScalar());
        });

        return habitId;
    }

    public List<Habit> GetAllHabits()
    {
        var habits = new List<Habit>();

        var commandText = $@"
            SELECT * FROM {habitTypesTableName}
        ";

        ExecuteQuery((command) =>
        {
            command.CommandText = commandText;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var habit = new Habit();

                    var habitId = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    var units = reader.GetString(2);

                    habit.Id = habitId;
                    habit.Name = name;
                    habit.Units = units;

                    habits.Add(habit);
                }
            }
        });

        return habits;
    }

    public void AddRecord(Data data)
    {
        var commandText = @$"
            INSERT INTO {habitsTableName} (Quantity, Date, HabitTypeId)
            VALUES (@quantity, @date, @habitTypeId);
            ";

        ExecuteQuery((command) =>
        {
            command.CommandText = commandText;
            command.Parameters.AddWithValue("@quantity", data.Quantity);
            command.Parameters.AddWithValue("@date", data.Date?.ToString(dateFormat));
            command.Parameters.AddWithValue("@habitTypeId", data.Habit.Id);
        });
    }

    public List<Data> GetAllRecords()
    {
        var allRecords = new List<Data>();

        var commandText = @$"
           SELECT
                h.Id AS HabitLogId,
                h.HabitTypeId,
                h.Quantity,
                h.Date,
                t.Id AS HabitId,
                t.Name AS HabitName,
                t.Units AS HabitUnits
            FROM {habitsTableName} h
            INNER JOIN {habitTypesTableName} t
                ON h.HabitTypeId = t.Id;
        ";

        ExecuteReader((command) =>
        {
            command.CommandText = commandText;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var dataItem = new Data();

                    var id = reader.GetInt32(reader.GetOrdinal("HabitLogId"));
                    dataItem.Id = id;

                    var habitId = reader.GetInt32(reader.GetOrdinal("HabitId"));
                    dataItem.Habit.Id = habitId;

                    var quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                    dataItem.Quantity = quantity;

                    var stringDate = reader.GetString(reader.GetOrdinal("Date"));

                    if (DateTime.TryParseExact(
                            stringDate,
                            dateFormat,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var parsedDate))
                    {
                        dataItem.Date = parsedDate;
                    }

                    var habitName = reader.GetString(reader.GetOrdinal("HabitName"));
                    dataItem.Habit.Name = habitName;

                    var habitUnits = reader.GetString(reader.GetOrdinal("HabitUnits"));
                    dataItem.Habit.Units = habitUnits;

                    allRecords.Add(dataItem);
                }
            }
        });

        return allRecords;
    }

    public void UpdateRecord(Data data)
    {
        var commandText = $@"
                UPDATE {habitsTableName}
                SET Quantity = @quantity,
                    Date = @date,
                    HabitTypeId = @habitTypeId
                WHERE Id = @id
            ";

        ExecuteQuery((command) =>
        {
            command.CommandText = commandText;
            command.Parameters.AddWithValue("@quantity", data.Quantity);
            command.Parameters.AddWithValue("@date", data.Date?.ToString(dateFormat));
            command.Parameters.AddWithValue("@habitTypeId", data.Habit.Id);
            command.Parameters.AddWithValue("@id", data.Id);
        });
    }

    public void DeleteRecord(int recordId)
    {
        var commandText = $@"
            DELETE FROM {habitsTableName}
            WHERE id = @id
        ";

        ExecuteQuery((command) =>
        {
            command.CommandText = commandText;
            command.Parameters.AddWithValue("@id", recordId);
        });
    }
    
    void SeedHabitsTypes()
    {
        ExecuteQuery(command =>
        {
            command.CommandText = @"
            INSERT OR IGNORE INTO habit_types (Id, Name, Units) VALUES
                (1, 'Water', 'liters'),
                (2, 'Running', 'km'),
                (3, 'Reading', 'pages'),
                (4, 'Workout', 'minutes');
        ";
        });
    }
    
    void SeedHabitLogs()
    {
        var count = ExecuteScalar<int>(command =>
        {
            command.CommandText = $"SELECT COUNT(*) FROM {habitsTableName};";
            return Convert.ToInt32(command.ExecuteScalar());
        });

        if (count > 0)
            return;

        var random = new Random();
        var startDate = DateTime.Today.AddDays(-100);

        for (int i = 0; i < 100; i++)
        {
            var habitTypeId = random.Next(1, 5);
            var quantity = random.Next(1, 11);
            var date = startDate.AddDays(i);

            ExecuteQuery(command =>
            {
                command.CommandText = $@"
                INSERT INTO {habitsTableName}
                    (HabitTypeId, Quantity, Date)
                VALUES
                    (@habitTypeId, @quantity, @date);
            ";

                command.Parameters.AddWithValue("@habitTypeId", habitTypeId);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@date", date.ToString(dateFormat));
            });
        }
    }

}