using System.Globalization;
using HabitLoggerApi.Models;
using Microsoft.Data.Sqlite;

namespace HabitLoggerApi;

public static class Database
{
  static string repoRoot = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..")
  );

  static string dbPath = Path.Combine(repoRoot, "HabitLogger.db");

  static string connectionString = new SqliteConnectionStringBuilder
  { DataSource = dbPath }.ToString();

  public static void InitializeDB()
  {
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = @"CREATE TABLE IF NOT EXISTS habit (
                              id INTEGER PRIMARY KEY AUTOINCREMENT,
                              name TEXT,
                              date TEXT,
                              quantity INTEGER
                            )";

    command.ExecuteNonQuery();

    connection.Close();
  }

  public static void CreateHabit(string? name, string date, int quantity)
  {
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = @$"INSERT INTO habit (name, date, quantity) VALUES ($name, $date, $quantity)";
    command.Parameters.AddWithValue("$name", name);
    command.Parameters.AddWithValue("$date", date);
    command.Parameters.AddWithValue("$quantity", quantity);

    command.ExecuteNonQuery();

    connection.Close();
  }

  public static List<Habit> QueryAllHabits()
  {
    List<Habit> habits = [];

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = @$"SELECT * FROM habit;";

    SqliteDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
      while (reader.Read())
      {
        habits.Add(
          new Habit
          {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Created_at = DateOnly.Parse(reader.GetString(2)),
            Quantity = reader.GetInt32(3)
          }
        );
      }
    }

    connection.Close();

    return habits;
  }

  public static bool UpdateQuery(int id, string newDate, int quantity)
  {
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();

    bool isHabitPresent = DoesHabitExist(id, command);
    if (!isHabitPresent)
    {
      connection.Close();
      return false;
    }

    command.Parameters.Clear();

    command.CommandText = @$"UPDATE habit SET date = $date, quantity = $quantity WHERE id = $id";
    command.Parameters.AddWithValue("$id", id);
    command.Parameters.AddWithValue("$date", newDate);
    command.Parameters.AddWithValue("$quantity", quantity);

    command.ExecuteNonQuery();

    connection.Close();

    return true;
  }

  public static bool DeleteQuery(int id)
  {
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();

    bool isHabitPresent = DoesHabitExist(id, command);

    if (!isHabitPresent)
    {
      connection.Close();
      return false;
    }

    command.Parameters.Clear();

    command.CommandText = @$"DELETE FROM habit WHERE id = $id";
    command.Parameters.AddWithValue("$id", id);

    command.ExecuteNonQuery();

    connection.Close();

    return true;
  }

  static bool DoesHabitExist(int id, SqliteCommand command)
  {
    command.CommandText = @$"SELECT EXISTS(SELECT 1 FROM habit WHERE id = $id)";
    command.Parameters.AddWithValue("$id", id);
    int checkQuery = Convert.ToInt32(command.ExecuteScalar());

    if (checkQuery == 0)
    {
      return false;
    }

    return true;
  }

  public static int HabitRecordsLength()
  {
    return QueryAllHabits().Count;
  }
}