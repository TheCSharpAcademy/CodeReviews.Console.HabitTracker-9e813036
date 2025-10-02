using System;
using error_messages;
using Microsoft.Data.Sqlite;

namespace sql_management
{
    public class SqlGenerator
    {
        public static void GenerateSqlTables(string connectionString)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var habitsCmd = connection.CreateCommand();

                    habitsCmd.CommandText =
                        $@"CREATE TABLE IF NOT EXISTS habits (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Type TEXT NOT NULL
                        );";

                    habitsCmd.ExecuteNonQuery();


                    var recordsCmd = connection.CreateCommand();

                    recordsCmd.CommandText =
                        $@"CREATE TABLE IF NOT EXISTS records (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            HabitId INTEGER NOT NULL,
                            Date TEXT NOT NULL,
                            Quantity INTEGER NOT NULL,
                            Type TEXT NOT NULL,
                            FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                        );";

                    recordsCmd.ExecuteNonQuery();

                    connection.Close();
                }
                Console.WriteLine("Database and tables created successfully. (Habits and Records)");
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error generating SQL tables: {ex.Message}");
                throw;
            }

        }
    }
}