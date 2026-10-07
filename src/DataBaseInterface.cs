using System.Globalization;

namespace HabitTracker;
using Microsoft.Data.Sqlite;
using CrypticWizard.RandomWordGenerator;

public static class DataBaseInterface
{

    private const string DbConnectionString = 
        "Data Source=HabitTracker.db;";
    private const string DbDateFormat = "yyyy-MM-dd HH:mm:ss";

    static DataBaseInterface()
    {
        CreateTable();
    }
    
    private static void CreateTable()
    {
        using var connection = new SqliteConnection(DbConnectionString);
        connection.Open();

        string createTablesQuery = @"
            CREATE TABLE IF NOT EXISTS Habits (
                Id INTEGER PRIMARY KEY,
                Name TEXT NOT NULL UNIQUE,
                Unit TEXT
            );

            CREATE TABLE IF NOT EXISTS HabitRecords (
                Id INTEGER PRIMARY KEY,
                HabitId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                Date TEXT,
                FOREIGN KEY (HabitId) REFERENCES Habits(Id) ON DELETE CASCADE
            );";

        using var command = new SqliteCommand(createTablesQuery, connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Tables created");
    }

    public static void InsertHabitRecord(HabitRecord habitRecord)
    {
        using var connection = new SqliteConnection(DbConnectionString);
        connection.Open();
        string insertQuery = "INSERT INTO HabitRecords (HabitId, Quantity, Date) VALUES (@HabitId, @Quantity, @Date)";
        
        if (habitRecord.Habit?.Id == null)
        {
            throw new InvalidOperationException("Cannot insert a HabitRecord without an Habit");
        }
        
        using var command = new SqliteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@HabitId", habitRecord.Habit.Id.Value);
        command.Parameters.AddWithValue("@Quantity", (object?)habitRecord.Quantity ?? DBNull.Value);
        command.Parameters.AddWithValue("@Date", ToDataBaseFormat(habitRecord.Date));
        
        command.ExecuteNonQuery();
    }

    public static Habit? GetHabitByName(string name)
    {
        using var connection = new SqliteConnection(DbConnectionString);
        connection.Open();
        string getQuery = "SELECT Id, Name, Unit FROM Habits WHERE LOWER(Name) = LOWER(@Name)";
        
        using var command = new SqliteCommand(getQuery, connection);
        command.Parameters.AddWithValue("@Name", name);
        
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Habit(
                id: reader.GetInt32(0),
                name: reader.GetString(1),
                unit: reader.IsDBNull(2) ? "" : reader.GetString(2)
            );
        }
        // If the query return nothing ie. doesn't exist
        return null;
    }

    public static void InsertHabit(Habit habit)
    {
        using var connection = new SqliteConnection(DbConnectionString);
        connection.Open();
        string insertQuery = "INSERT INTO Habits (Name, Unit) VALUES (@Name, @Unit);" +
                             "SELECT last_insert_rowid();";
        
        using var command = new SqliteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@Name", habit.Name);
        command.Parameters.AddWithValue("@Unit", (object?)habit.Unit ?? DBNull.Value);

        long id = (long)command.ExecuteScalar()!;
        habit.Id = (int)id;
        //command.ExecuteNonQuery();   
    }
    
    public static List<HabitRecord> ViewHabits(int? id = null, string? name = null, DateTime? startTime = null, DateTime? endTime = null)
    {
        var habits = new List<HabitRecord>();
        var setClauses = new List<String>();
        using var command = new SqliteCommand();

        if (id.HasValue)
        {
            setClauses.Add("record.Id = @Id");
            command.Parameters.AddWithValue("@Id", id.Value);
        }
        if (!string.IsNullOrEmpty(name))
        {
            setClauses.Add("habit.Name LIKE @Name");
            command.Parameters.AddWithValue("@Name", $"%{name}%");
        }
        if (startTime.HasValue)
        {
            setClauses.Add("record.Date >= @StartTime");
            command.Parameters.AddWithValue("@StartTime", ToDataBaseFormat(startTime.Value));
        }
        if (endTime.HasValue)
        {
            setClauses.Add(("record.Date <= @EndTime"));
            command.Parameters.AddWithValue("@EndTime", ToDataBaseFormat(endTime.Value));
        }

        string baseViewQuery = @"
            SELECT
                record.Id,
                record.Quantity, 
                record.Date,
                habit.Id AS HabitId,
                habit.Name,
                Habit.Unit
            FROM HabitRecords record
            INNER JOIN Habits habit ON record.HabitId = habit.Id";
        
        command.CommandText = setClauses.Count > 0
            ? $"{baseViewQuery} WHERE {string.Join(" AND ", setClauses)}"
            : baseViewQuery;
        
        using var connection = new SqliteConnection(DbConnectionString);
        command.Connection = connection;
        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var habit = new Habit(
                id: reader.GetInt32(3),
                name: reader.GetString(4),
                unit: reader.IsDBNull(5) ? "" : reader.GetString(5));
            habits.Add(new HabitRecord(
                id: reader.GetInt32(0),
                habit: habit, 
                quantity: reader.GetInt32(1), 
                date: reader.IsDBNull(2) ? null : FromDataBaseFormat(reader.GetString(2))));
        }
        
        return habits;
    }

    public static void UpdateHabit(HabitRecord habitRecord)
    {
        var setClauses = new List<String>();
        using var command = new SqliteCommand();

        if (habitRecord.Quantity.HasValue)
        {
            setClauses.Add("Quantity = @Quantity");
            command.Parameters.AddWithValue("@Quantity", habitRecord.Quantity.Value);
        }
        if (habitRecord.Date.HasValue)
        {
            setClauses.Add("Date = @Date");
            command.Parameters.AddWithValue("@Date", habitRecord.Date.Value.ToString("yyyy-MM-dd HH:mm;ss"));
        }
        
        if (setClauses.Count == 0) return;
        
        string updateQuery = $@"
            UPDATE HabitRecords
            SET {string.Join(", ", setClauses)}
            WHERE Id = @Id";
        command.CommandText = updateQuery;
        
        using var connection = new SqliteConnection(DbConnectionString);
        command.Connection = connection;
        connection.Open();

        command.Parameters.AddWithValue("@Id", habitRecord.Id);

        command.ExecuteNonQuery();
    }

    public static void DeleteHabit(int habitId)
    {
        using var connection = new SqliteConnection(DbConnectionString);
        connection.Open();
        string deleteQuery = "DELETE FROM HabitRecords WHERE Id = @Id";

        using var command = new SqliteCommand(deleteQuery, connection);
        command.Parameters.AddWithValue("@Id", habitId);

        command.ExecuteNonQuery();
    }
    
    public static async Task PopulateTableWithRandomData()
    {
        int amountToAdd = 5;
        DateTime?[] randomDate = [DateTime.Now, DateTime.MaxValue, DateTime.UnixEpoch, new DateTime(2025, 12 ,25)];
        string?[] randomUnit = [null, "km", "pages", "h", "cm²/S"];
        var numberGenerator = new Random();
        var wordGenerator = new WordGenerator();
        for (var i = 0; i < amountToAdd; i++)
        {
            var habit = new Habit(wordGenerator.GetWord(), randomUnit[numberGenerator.Next(randomUnit.Length)]);
            InsertHabit(habit);
            var amountOfRecords = numberGenerator.Next(1, amountToAdd * 2);
            
            for (var j = 0; j < amountOfRecords; j++)
            {
                InsertHabitRecord(new HabitRecord(
                    habit: habit, 
                    quantity: numberGenerator.Next(100),
                    date: randomDate[numberGenerator.Next(randomDate.Length)]));
            }
        }
        
        Console.WriteLine($"Populating the DB with {amountToAdd} habits, for testing & review purposes");
    }

    private static object ToDataBaseFormat(DateTime? date)
    {
        return date.HasValue ? date.Value.ToString(DbDateFormat, CultureInfo.InvariantCulture) : DBNull.Value;
    }

    // dateString must respect DbDateFormat format
    private static DateTime? FromDataBaseFormat(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString)) return null;

        if (DateTime.TryParseExact(dateString, DbDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var parsedDateTime))
        {
            return parsedDateTime;
        }
        
        // If TryParseExact fails, it means we have wrong or old data in db, fallback to generic parser
        return DateTime.Parse(dateString, CultureInfo.InvariantCulture);
    }
}