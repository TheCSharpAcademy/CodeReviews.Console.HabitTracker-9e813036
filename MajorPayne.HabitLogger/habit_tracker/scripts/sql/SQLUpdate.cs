using System;
using habit_tracker;
using menu_manager;
using error_messages;

namespace sql_management
{
    public class SQLUpdate
    {
        public static void UpdateRecord(string connectionString, string tableName)
        {
            Console.Clear();
            SQLRead sqlRead = new SQLRead(connectionString);
            sqlRead.ViewAllRecords(tableName);

            MenuManager.UpdateMenu();
            var recordId = Convert.ToInt32(InputManager.GetUserInput());
            if (recordId == 0) return;

            MenuManager.DateMenu();
            string date = InputManager.GetDateInput();

            MenuManager.WaterMenu();
            int quantity = Convert.ToInt32(InputManager.GetUserInput());

            try
            {
                SQLDatabaseHelper.ExecuteNonQuery(
                    connectionString,
                    $"UPDATE [{tableName}] SET date = @Date, quantity = @Quantity WHERE Id =@Id;",
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@Id", recordId);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                    }
                );
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("An error occurred while updating the record: " + ex.Message);
                throw;
            }
        }

        public static void UpdateHabit(string connectionString)
        {
            Console.Clear();
            SQLRead sqlRead = new SQLRead(connectionString);
            sqlRead.ViewAllHabits();

            MenuManager.UpdateMenu();
            var habitId = Convert.ToInt32(InputManager.GetUserInput());
            if (habitId == 0) return;

            MenuManager.HabitNameMenu();
            string habitName = InputManager.GetHabitInput();

            MenuManager.HabitTypeMenu();
            string habitType = InputManager.GetHabitInput();

            try
            {
                SQLDatabaseHelper.ExecuteNonQuery(
                    connectionString,
                    $"UPDATE habits SET name = @Name, type = @Type WHERE Id =@Id;",
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@Id", habitId);
                        cmd.Parameters.AddWithValue("@Name", habitName);
                        cmd.Parameters.AddWithValue("@Type", habitType);
                    }
                );
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("An error occurred while updating the habit: " + ex.Message);
                throw;
            }
        }
    }
}