namespace HabitTracker.CSamu7
{
    public class MainMenu
    {
        private readonly List<string> _options = new List<string>()
        {
            "Create a habit", "Check your habits", "Log a habit",
            "Check all the logs", "Modify a log", "Delete a log"
        };
        public void DisplayMenu()
        {
            Console.WriteLine("Enter an option");

            for(int i = 0; i<_options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_options[i]}");
            }

            Console.Write("Enter your option: ");
            int option = ConsoleUtils.ReadInt();

            ExecuteOption(option);
            Console.Clear();
        }
        public void ExecuteOption(int option)
        {
            HabitControllers habitCreation = new();

            switch (option)
            {
                case 1:
                    habitCreation.CreateHabit();
                    break;
                case 2:
                    habitCreation.GetHabits();
                    break;
                case 3:
                    //Log a habit
                    break;
                case 4:
                    //Check all the logs
                    break;
                case 5:
                    //Modify a log
                    break;
                case 6:
                    //Delete a log
                    break;
                default:
                    Console.WriteLine("This option doesn't exist");
                    break;

            }
            Console.WriteLine("");
        }
    }
}
