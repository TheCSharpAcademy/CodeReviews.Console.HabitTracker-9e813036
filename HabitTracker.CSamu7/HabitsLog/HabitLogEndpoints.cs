using HabitTracker.CSamu7.Habits;

namespace HabitTracker.CSamu7.HabitsLog
{
    public class HabitLogEndpoints
    {
        private readonly HabitService _habitsService = new HabitService();
        private readonly HabitLogService _habitsLogService = new HabitLogService();
        private readonly HabitCreation _habitCreation;

        public HabitLogEndpoints()
        {
            _habitCreation = new HabitCreation(TimeProvider.System);
        }
        public async Task CreateHabit()
        {
            Console.WriteLine("YOUR HABITS");
            Console.WriteLine("==============================================");
            List<HabitModel> habits = await _habitsService.GetHabits();

            foreach (var habit in habits)
            {
                Console.WriteLine($"{habit.Id}. {habit.Name}");
            }

            Console.WriteLine("==============================================");

            PostHabitLogModel newHabit = await _habitCreation.CreateHabit();
            _habitsLogService.CreateHabitLog(newHabit);

            ConsoleUtils.WriteLine("Your habit has been added", ConsoleColor.Green);
            ConsoleUtils.WaitKey();
        }
    }
}
