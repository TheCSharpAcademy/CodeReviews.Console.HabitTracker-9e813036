using Microsoft.Data.Sqlite;
using error_messages;

namespace sql_management
{
    public static class SqlDatabaseHelper
    {
        public static List<T> ExecuteQuery<T>(
            string connectionString,
            string sql,
            Func<SqliteDataReader, T> mapFunction,
            Action<SqliteCommand>? parameterize = null
        )
        {
            var results = new List<T>();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = sql;

                    parameterize?.Invoke(command);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(mapFunction(reader));
                            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetString(2)}");
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("Database query error: " + ex.Message);
                throw;
            }
        }

        public static int ExecuteNonQuery(
            string connectionString,
            string sql,
            Action<SqliteCommand>? parameterize = null
        )
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;

                parameterize?.Invoke(command);

                return command.ExecuteNonQuery();
            }
        }

        public static string GenerateTableName(string habitName)
        {
            string safe = new string(habitName
                .ToLower()
                .Replace(' ', '_')
                .ToCharArray());

            try
            {
                safe = System.Text.RegularExpressions.Regex.Replace(safe, @"[^a-z0-9_]", "");
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error generating table name: {ex.Message}");
                throw;
            }

            return $"habit_{safe}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        }

        public static string? GetHabitType(string connectionString, string tableName)
        {
            string? type = null;
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT Type FROM habits WHERE TableName = @tableName;";
                    cmd.Parameters.AddWithValue("@tableName", tableName);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        type = result.ToString();
                }
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error retrieving habit type: {ex.Message}");
                throw;
            }


            return type;
        }
    }

}