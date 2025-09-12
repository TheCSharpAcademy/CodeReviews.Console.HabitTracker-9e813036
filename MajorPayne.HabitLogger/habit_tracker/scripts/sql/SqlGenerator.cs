using System;
using error_messages;
using Microsoft.Data.Sqlite;

namespace sql_management
{
    public class SqlGenerator
    {
        public static void GenerateSqlTable(string connectionString, string tableName)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        $@"CREATE TABLE IF NOT EXISTS [{tableName}] (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        date TEXT,
                        quantity INTEGER,
                        type TEXT
                        );";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error generating SQL table: {ex.Message}");
                throw;
            }

        }

        public static void GenerateHabitTable(string connectionString)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        type TEXT NOT NULL,
                        tableName TEXT NOT NULL UNIQUE
                        );";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error generating habit table: {ex.Message}");
                throw;
            }

        }
    }
}