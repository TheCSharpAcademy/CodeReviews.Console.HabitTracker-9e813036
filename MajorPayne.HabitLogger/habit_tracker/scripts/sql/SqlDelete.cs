using habit_tracker;
using menu_manager;
using error_messages;

namespace sql_management
{
    public class SqlDelete
    {
        public static void DeleteRecord(string connectionString, string tableName)
        {
            Console.Clear();
            SqlRead sqlRead = new SqlRead(connectionString);
            sqlRead.ViewAllRecords(tableName);

            MenuManager.DeleteMenu();
            var recordId = Convert.ToInt32(InputManager.GetUserInput());
            if (recordId == 0) return;

            try
            {
                SqlDatabaseHelper.ExecuteNonQuery(
                    connectionString,
                    $"DELETE FROM [{tableName}] WHERE Id = @Id;",
                    cmd => cmd.Parameters.AddWithValue("@Id", recordId)
                );
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("An error occurred while deleting the record: " + ex.Message);
                throw;
            }
        }

        public static void DeleteHabit(string connectionString)
        {
            Console.Clear();
            SqlRead sqlRead = new SqlRead(connectionString);
            sqlRead.ViewAllHabits();

            MenuManager.DeleteMenu();
            var habitId = Convert.ToInt32(InputManager.GetUserInput());
            if (habitId == 0) return;

            try
            {
                SqlDatabaseHelper.ExecuteNonQuery(
                    connectionString,
                    $"DELETE FROM Habits WHERE Id = @Id;",
                    cmd => cmd.Parameters.AddWithValue("@Id", habitId)
                );
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("An error occurred while deleting the habit: " + ex.Message);
                throw;
            }
        }
    }  
}