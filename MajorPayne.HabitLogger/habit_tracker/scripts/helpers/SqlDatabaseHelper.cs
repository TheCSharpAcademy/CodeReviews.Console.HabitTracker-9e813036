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

        public static string GetHabitType(string connectionString, int habitId)
        {
            string type = "unknown";
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT Type FROM habits WHERE Id = @Id;";
                    cmd.Parameters.AddWithValue("@Id", habitId);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        type = result.ToString() ?? "unknown";
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