using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.CSamu7.Habits
{
    public class HabitEndpoints
    {
        private readonly HabitService _service = new HabitService();
        public async void CreateHabit()
        {
            Console.Write("Enter the name: ");
            string name = ConsoleUtils.ReadString();

            HabitService service = new HabitService();
            bool isSuccess = await service.PostHabit(name);

            if (isSuccess) {
                Console.WriteLine($"The habit {name} has been added!");
            } else
            {
                Console.WriteLine("Something happened...");
            }

            Console.WriteLine("Enter any key to continue");
            Console.ReadKey();
        }
        public async void GetHabits()
        {
            List<HabitModel> habits = await _service.GetHabits();

            if (habits.Count > 0)
            {
                Console.WriteLine("ID\t| Name");
                Console.WriteLine(new String('=', 80));

                foreach (HabitModel habit in habits)
                {
                    Console.WriteLine($"{habit.Id}\t| {habit.Name}");
                }
            } else
            {
                Console.WriteLine("You don't have habits created");
            }

            Console.WriteLine("Enter any key to continue");
            Console.ReadKey();
        }
    }
}
