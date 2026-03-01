using HabitTracker.CSamu7.Habits;
using HabitTracker.CSamu7.HabitsLog;
using HabitTracker.CSamu7.Utils;

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
            ConsoleUtils.WriteTitle("HABITS TRACKER");
            Console.WriteLine("Enter an option");

            for (int i = 0; i < _options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_options[i]}");
            }

            int option = ConsoleUtils.ReadInt("Enter the option: ");
            ExecuteOption(option);
        }
        private async void ExecuteOption(int option)
        {
            HabitEndpoints habitEndpoints = new();
            HabitLogEndpoints habitLogEndpoints = new();
            Console.Clear();

            switch (option)
            {
                case 1:
                    await habitEndpoints.Create();
                    break;
                case 2:
                    await habitEndpoints.GetAll();
                    break;
                case 3:
                    await habitLogEndpoints.Create();
                    break;
                case 4:
                    await habitLogEndpoints.GetAll();
                    break;
                case 5:
                    await habitLogEndpoints.Modify();
                    break;
                case 6:
                    await habitLogEndpoints.Delete();
                    break;
                default:
                    ConsoleUtils.WriteError("This option doesn't exist");
                    break;
            }

            ConsoleUtils.WaitKey();
            Console.Clear();
        }
    }
}
