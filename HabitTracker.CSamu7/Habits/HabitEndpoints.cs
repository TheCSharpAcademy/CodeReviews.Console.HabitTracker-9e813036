using HabitTracker.CSamu7.Utils;

namespace HabitTracker.CSamu7.Habits
{
    public class HabitEndpoints
    {
        private readonly HabitService _service = new();
        public async Task Create()
        {
            ConsoleUtils.WriteTitle("HABIT CREATION");

            Console.Write("Enter the name: ");
            string name = ConsoleUtils.ReadString();

            bool isSuccess = await _service.Create(name);

            if (isSuccess)
            {
                ConsoleUtils.WriteSuccess($"The habit {name} has been added!");
            } else
            {
                ConsoleUtils.WriteError("The habit can't be added");
            }
        }
        public async Task GetAll()
        {
            ICollection<HabitModel> habits = await _service.GetAll();

            if (habits.Count > 0)
            {
                Console.WriteLine(new String('=', 80));
                Console.WriteLine("{0,-5}| {1,-20}", "Id", "Name");
                Console.WriteLine(new String('=', 80));

                foreach (HabitModel habit in habits)
                {
                    Console.WriteLine("{0,-5}| {1,-20}", habit.Id, habit.Name);
                }
            } else
            {
                Console.WriteLine("You don't have any habits created");
            }
        }
    }
}
