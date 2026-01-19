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
    using (var connection = new SqliteConnection(connectionString))
    {
      connection.Open();

      var command = connection.CreateCommand();
      command.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                              Id INTEGER PRIMARY KEY AUTOINCREMENT,
                              Date TEXT,
                              Quantity INTEGER
                            )";

      command.ExecuteNonQuery();

      connection.Close();
    }
  }
}