using System.Globalization;
using error_messages;
using menu_manager;

namespace habit_tracker
{
    public class InputManager
    {
        public static string GetUserInput()
        {
            string? input = Console.ReadLine();
            if (input == null) GetUserInput();

            if (input == ".test") return ".test";

            while (!int.TryParse(input, out _) || Convert.ToInt32(input) < 0)
            {
                DisplayError.ErrorMessage("invalid_input");
                input = Console.ReadLine();
            }

            return input;
        }

        public static string GetDateInput()
        {
            string? date = Console.ReadLine();
            if (date == null) GetDateInput();

            if (date == "0") GetDateInput();

            while (!DateTime.TryParseExact(date, "MM-dd-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                DisplayError.ErrorMessage("invalid_date");
                date = Console.ReadLine();
            }

            return date;
        }

        public static string GetHabitInput()
        {
            string? name = Console.ReadLine();
            if (name == null) GetHabitInput();

            while (string.IsNullOrWhiteSpace(name))
            {
                DisplayError.ErrorMessage("invalid_input");
                name = Console.ReadLine();
            }

            return name;
        }

        public static string GetHabitSelection()
        {
            string? choice = Console.ReadLine();
            if (choice == null) GetHabitSelection();

            while (string.IsNullOrWhiteSpace(choice))
            {
                DisplayError.ErrorMessage("invalid_input");
                choice = Console.ReadLine();
            }

            if (choice == "0") MenuManager.MainMenu();
 
            return choice;
        }
    }
}