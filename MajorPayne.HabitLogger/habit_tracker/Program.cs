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
            SqlGenerator.GenerateSqlTables(connectionString);

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
                            int habitId = HabitSelector.GetHabitIdFromUserSelection(sqlReader);
                            if (habitId > 0)
                                DisplayHabitRecords(connectionString, habitId);
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

        public static void DisplayHabitRecords(string connectionString, int habitId)
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
                            sqlReader.ViewAllRecords(habitId);
                            Console.ReadLine(); // Pause to let user read records
                            break;
                        case "2":
                            SqlCreate.CreateRecord(connectionString, habitId);
                            break;
                        case "3":
                            SqlUpdate.UpdateRecord(connectionString, habitId);
                            break;
                        case "4":
                            SqlDelete.DeleteRecord(connectionString, habitId);
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
