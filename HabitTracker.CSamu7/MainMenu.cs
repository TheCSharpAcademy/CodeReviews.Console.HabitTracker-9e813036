using HabitTracker.CSamu7.Habits;
using HabitTracker.CSamu7.HabitsLog;
using System.Threading.Tasks;

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
        public async void ExecuteOption(int option)
        {
            HabitEndpoints habitEndpoints = new();
            HabitLogEndpoints habitLogEndpoints = new();

            switch (option)
            {
                case 1:
                    habitEndpoints.CreateHabit();
                    break;
                case 2:
                    habitEndpoints.GetHabits();
                    break;
                case 3:
                    await habitLogEndpoints.CreateLogHabit();
                    break;
                case 4: 
                    await habitLogEndpoints.GetLogHabits();
                    break;
                case 5:
                    await habitLogEndpoints.ModifyLog();
                    break;
                case 6:
                    await habitLogEndpoints.DeleteTask();
                    break;
                default:
                    Console.WriteLine("This option doesn't exist");
                    break;

            }
            Console.WriteLine("");
        }
    }
}
