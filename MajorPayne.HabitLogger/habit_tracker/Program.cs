using System;
using menu_manager;
using sql_management;
using error_messages;

namespace habit_tracker
{
    class Program
    {
        static readonly string connectionString = @"Data Source=habit-tracker.db";

        static void Main()
        {
            SqlGenerator.GenerateHabitTable(connectionString);

            SqlRead sqlReader = new SqlRead(connectionString);
            bool closeApp = false;

            while (!closeApp)
            {
                try
                {
                    MenuManager.MainMenu();
                    string? choice = InputManager.GetUserInput();

                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("Closing application...\n");
                            closeApp = true;
                            break;
                        case "1":
                            string? tableName = HabitSelector.GetTableNameFromUserSelection(sqlReader);
                            if (tableName != null)
                                DisplayHabitRecords(connectionString, tableName);
                            break;
                        case "2":
                            SqlCreate.CreateHabit(connectionString);
                            break;
                        case "3":
                            SqlUpdate.UpdateHabit(connectionString);
                            break;
                        case "4":
                            SqlDelete.DeleteHabit(connectionString);
                            break;
                        case ".test":
                            Console.WriteLine("Generating Test Data...");
                            DatabaseSeeder.Seed(connectionString);
                            break;
                        default:
                            DisplayError.ErrorMessage("invalid_choice");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError.ErrorMessage("An error occurred in Main: " + ex.Message);
                }


            }
        }

        public static void DisplayHabitRecords(string connectionString, string tableName)
        {
            SqlRead sqlReader = new SqlRead(connectionString);

            while (true)
            {
                try
                {
                    MenuManager.HabitMenu();
                    string? choice = InputManager.GetUserInput();

                    switch (choice)
                    {
                        case "0":
                            return; // Return to main menu
                        case "1":
                            sqlReader.ViewAllRecords(tableName);
                            Console.ReadLine(); // Pause to let user read records
                            break;
                        case "2":
                            SqlCreate.CreateRecord(connectionString, tableName);
                            break;
                        case "3":
                            SqlUpdate.UpdateRecord(connectionString, tableName);
                            break;
                        case "4":
                            SqlDelete.DeleteRecord(connectionString, tableName);
                            break;
                        default:
                            DisplayError.ErrorMessage("invalid_choice");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError.ErrorMessage("An error has occurred in HabitMenu: " + ex.Message);
                }
            }
        }
    }
}
