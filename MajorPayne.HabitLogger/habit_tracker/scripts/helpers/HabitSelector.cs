using error_messages;
using sql_management;

namespace habit_tracker
{
    public static class HabitSelector
    {
        public static int GetHabitIdFromUserSelection(SqlRead sqlReader)
        {
            var habits = sqlReader.ViewAllHabits();
            if (habits.Count == 0)
            {
                Console.WriteLine("No habits found.");
                return 0;
            }

            Console.WriteLine("\nSelect a habit by number (0 to return to main menu): ");
            int index = -1;

            while (true)
            {
                string input = InputManager.GetUserInput();

                if (input == "0")
                    return 0; // Return to main menu

                if (int.TryParse(input, out index) && index >= 1 && index <= habits.Count)
                    break;

                DisplayError.ErrorMessage("invalid_choice");
            }

            // Return actual table name
            return habits[index - 1].Id;
        }
    }
}
