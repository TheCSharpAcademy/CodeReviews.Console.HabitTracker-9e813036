namespace HabitTracker.CSamu7
{
    public class HabitControllers
    {
        private readonly HabitService _service = new HabitService();
        public void CreateHabit()
        {
            Console.WriteLine("Enter the name of the habit");
            string name = ConsoleUtils.ReadString();

            bool isSuccess = _service.PostHabit(name);

            if (isSuccess) {
                Console.WriteLine($"The habit {name} has been added!");
            } else
            {
                Console.WriteLine("Something happened...");
            }
        }
        public void GetHabits()
        {
            List<Habit> habits = _service.GetHabits();

            if (habits.Count > 0)
            {
                Console.WriteLine("ID\t|\tName");

                foreach(Habit habit in habits)
                {
                    Console.WriteLine($"{habit.Id}\t|\t{habit.Name}");
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
