using System;
using habit_tracker;
using menu_manager;
using error_messages;

namespace sql_management
{
    public class SQLCreate
    {
        // ✅ Low-level habit insert with error handling
        public static void InsertHabit(string connectionString, string name, string type, string tableName)
        {
            try
            {
                SQLDatabaseHelper.ExecuteNonQuery(
                    connectionString,
                    $"INSERT INTO habits(name, type, tableName) VALUES (@name, @type, @tableName);",
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@tableName", tableName);
                    }
                );

                SQLGenerator.GenerateSQLTable(connectionString, tableName);

                Console.WriteLine($"Habit '{name}' created successfully with table '{tableName}'.");
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error inserting habit '{name}': {ex.Message}");
            }
        }

        // ✅ Low-level record insert with error handling
        public static void InsertRecord(string connectionString, string tableName, string date, int quantity, string type)
        {
            try
            {
                SQLDatabaseHelper.ExecuteNonQuery(
                    connectionString,
                    $"INSERT INTO [{tableName}] (date, quantity, type) VALUES (@Date, @Quantity, @Type);",
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@Type", type);
                    }
                );

                Console.WriteLine($"Record added to '{tableName}' on {date}: {quantity} {type}.");
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error inserting record into '{tableName}': {ex.Message}");
            }
        }

        // ✅ UI-driven habit creation with error handling
        public static void CreateHabit(string connectionString)
        {
            Console.Clear();
            try
            {
                MenuManager.HabitNameMenu();
                string name = InputManager.GetHabitInput();

                MenuManager.HabitTypeMenu();
                string type = InputManager.GetHabitInput();

                string tableName = SQLDatabaseHelper.GenerateTableName(name);

                InsertHabit(connectionString, name, type, tableName);
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error creating habit: {ex.Message}");
            }
        }

        // ✅ UI-driven record creation with error handling
        public static void CreateRecord(string connectionString, string tableName)
        {
            Console.Clear();
            try
            {
                MenuManager.DateMenu();
                string date = InputManager.GetDateInput();

                MenuManager.WaterMenu();
                string quantityInput = InputManager.GetUserInput();
                if (!int.TryParse(quantityInput, out int quantity))
                {
                    Console.WriteLine("Invalid quantity. Record not created.");
                    return;
                }

                string type = SQLDatabaseHelper.GetHabitType(connectionString, tableName) ?? "units";

                InsertRecord(connectionString, tableName, date, quantity, type);
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage($"Error creating record: {ex.Message}");
            }
        }
    }
}
