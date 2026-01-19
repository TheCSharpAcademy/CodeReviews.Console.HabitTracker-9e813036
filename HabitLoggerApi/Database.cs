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
}