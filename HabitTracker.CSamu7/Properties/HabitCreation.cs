using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.CSamu7.Properties
{
    public class HabitCreation
    {
        public void CreateHabit()
        {
            Console.WriteLine("Enter the name of the habit");
            string name = ConsoleUtils.ReadString();

            HabitService service = new HabitService();
            bool isSuccess = service.PostHabit(name);

            if (isSuccess) {
                Console.WriteLine($"The habit {name} has been added!");
            } else
            {
                Console.WriteLine("Something happened...");
            }
        }
    }
}
