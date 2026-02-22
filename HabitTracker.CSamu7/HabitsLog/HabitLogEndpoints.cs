using HabitTracker.CSamu7.Habits;
using System;

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
        public async Task CreateLogHabit()
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
        public async Task GetLogHabits()
        {
            ICollection<HabitLogModel> habits = await _habitsLogService.GetHabitLogs();

            if (habits.Count > 0)
            {
                Console.WriteLine(new String('=', 80));
                Console.WriteLine("{0,-5}| {1,-20}| {2,-10}| {3,-10}", "Id", "Name", "Amount", "Date");
                Console.WriteLine(new String('=', 80));

                foreach (HabitLogModel habit in habits)
                {
                    DateTimeOffset datetime = DateTimeOffset.FromUnixTimeSeconds(habit.UnixTimeStamp).ToLocalTime();

                    Console.WriteLine("{0,-5}| {1,-20}| {2,-10}| {3,-10:d}",
                        habit.Id, habit.HabitName, habit.Amount, datetime);
                }
            } else
            {
                Console.WriteLine("You don't have habits created");
            }

            ConsoleUtils.WaitKey();
        }
    }
}
