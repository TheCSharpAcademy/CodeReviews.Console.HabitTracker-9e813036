using Microsoft.Data.Sqlite;
using System.Data;

namespace Habit_Tracker
{
    internal static class DatabaseManager
    {
        private static readonly string _connectionString = "Data Source=habit-tracker.db;Foreign Keys=True";

        public static SqliteConnection OpenConnection()
        {
            SqliteConnection connection = new(_connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                connection.Dispose();
                Logger.Log($"Error: {e.Message}");
                throw;
            }
        }

        public static string FormatParameters(SqliteParameterCollection parameters)
        {
            if (parameters.Count == 0)
                return "(none)";

            List<string> parts = new();
            foreach(SqliteParameter parameter in parameters)
            {
                parts.Add($"{parameter.ParameterName}={parameter.Value}");
            }
            return string.Join(", ", parts);
        }

        public static void ExecuteNonQuery(SqliteConnection connection, string sql, (string, object)[]? parameters = null)
        {
            try
            {
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }
                }
                Logger.Log($"Executing command: {command.CommandText} | Parameters: {FormatParameters(command.Parameters)}");
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.Log($"Error: {e.Message}");
                throw;
            }
        }

        public static int ExecuteScalar(SqliteConnection connection, string sql, (string, object)[]? parameters = null)
        {
            try
            {
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }
                }
                Logger.Log($"Executing: {command.CommandText} | Parameters: {FormatParameters(command.Parameters)}");
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception e)
            {
                Logger.Log($"Error: {e.Message}");
                throw;
            }
        }


        public static SqliteDataReader GetReader(SqliteConnection connection, string sql, (string, object)[]? parameters = null)
        {
            SqliteCommand command = connection.CreateCommand();
            try
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }
                }
                Logger.Log($"Executing command: {command.CommandText}");
                SqliteDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                Logger.Log($"Error: {e.Message}");
                command.Dispose();
                throw;
            }
        }

        public static DataTable GetFullTable(string tableName)
        {
            using SqliteConnection connection = OpenConnection();
            using SqliteDataReader reader = GetReader(connection, $"SELECT * FROM {tableName}");
            DataTable table = new();
            table.Load(reader);
            return table;
        }

        public static void Start()
        {
            using SqliteConnection connection = OpenConnection();
            ExecuteNonQuery(connection, @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT UNIQUE NOT NULL
                    )"
            );
            ExecuteNonQuery(connection, @"
                CREATE TABLE IF NOT EXISTS HabitEntries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    Quantity INTEGER DEFAULT 0 CHECK(Quantity >= 0),
                    FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                    )"
            );
        }

        public static int CreateHabit(string habitName)
        {
            using SqliteConnection connection = OpenConnection();
            int? result = ExecuteScalar(connection, 
                @"INSERT INTO Habits (Name) VALUES (@name) RETURNING Id", 
                [
                    ("@name", habitName)
                ]
            );
            return result.Value;
        }

        public static void InsertHabitEntry(int id, string date, int quantity)
        {
            using SqliteConnection connection = OpenConnection();
            ExecuteNonQuery(connection, 
                @"INSERT INTO HabitEntries (HabitId, Date, Quantity) VALUES (@id, @date, @quantity)", 
                [
                    ("@id", id), 
                    ("@date", date), 
                    ("@quantity", quantity)
                ]
            );
        }

        public static void UpdateHabitEntry(int id, string date, int quantity)
        {
            using SqliteConnection connection = OpenConnection();
            ExecuteNonQuery(connection,
                @"UPDATE HabitEntries SET Date = @date, Quantity = @quantity WHERE Id = @id",
                [
                    ("@id", id),
                    ("@date", date),
                    ("@quantity", quantity)
                ]
            );
        }

        public static void DeleteHabit(int id)
        {
            using SqliteConnection connection = OpenConnection();
            ExecuteNonQuery(connection,
                @"DELETE FROM HabitEntries WHERE HabitId = @id",
                [
                    ("@id", id)
                ]
            );
            ExecuteNonQuery(connection,
                @"DELETE FROM Habits WHERE Id = @id",
                [
                    ("@id", id)
                ]
            );
        }

        public static void DeleteEntry(int id)
        {
            using SqliteConnection connection = OpenConnection();
            ExecuteNonQuery(connection,
                @"DELETE FROM HabitEntries WHERE Id = @id",
                [
                    ("@id", id)
                ]
            );
        }

        public static void DeleteEntriesOfHabit(int habitId)
        {
            using SqliteConnection connection = OpenConnection();
            ExecuteNonQuery(connection,
                @"DELETE FROM HabitEntries WHERE HabitId = @habitId",
                [
                    ("@habitId", habitId)
                ]
            );
        }

        public static List<(int Id, string Name)> GetHabits()
        {
            DataTable table = GetFullTable("Habits");
            List<(int Id, string Name)> habits = new();
            const int Id = 0;
            const int Name = 1;
            foreach(DataRow row in table.Rows)
            {
                habits.Add((Convert.ToInt32(row.ItemArray[Id]), (string)row.ItemArray[Name]));
            }
            return habits;
        }

        public static List<(int Id, string Name, int Total)> GetHabitTotals()
        {
            using SqliteConnection connection = OpenConnection();
            using SqliteDataReader reader = GetReader(connection, @"SELECT H.Id, H.Name, COALESCE(SUM(HE.Quantity), 0)
                                                                    FROM Habits as H
                                                                    LEFT JOIN HabitEntries as HE
                                                                    ON HE.HabitId = H.Id
                                                                    GROUP BY H.Id
                                                                    ORDER BY H.Id");
            List<(int Id, string Name, int Total)> habits = new();
            const int Id = 0;
            const int Name = 1;
            const int Total = 2;
            while (reader.Read())
            {
                habits.Add((reader.GetInt32(Id), reader.GetString(Name), reader.GetInt32(Total)));
            }
            return habits;
        }

        public static List<(int Id, string Name, string Date, int Quantity)> GetHabitEntries(int habitId = 0)
        {
            using SqliteConnection connection = OpenConnection();
            using SqliteDataReader reader = GetReader(connection,
                                                    @"SELECT HE.Id, H.Name, HE.Date, HE.Quantity 
                                                    FROM Habits as H 
                                                    INNER JOIN HabitEntries as HE 
                                                    ON H.Id = HE.HabitId
                                                    WHERE (@id = 0 OR H.Id = @id)
                                                    ORDER BY H.Id, HE.Date",
                                                    [
                                                        ("@id", habitId)
                                                    ]);
            List<(int Id, string Name, string Date, int Quantity)> habits = new();
            const int Id = 0;
            const int Name = 1;
            const int Date = 2;
            const int Quantity = 3;
            while (reader.Read())
            {
                habits.Add((reader.GetInt32(Id), reader.GetString(Name), reader.GetString(Date), reader.GetInt32(Quantity)));
            }
            return habits;
        }
    }
}