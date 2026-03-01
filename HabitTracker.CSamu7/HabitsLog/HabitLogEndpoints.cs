using HabitTracker.CSamu7.Habits;
using HabitTracker.CSamu7.Utils;

namespace HabitTracker.CSamu7.HabitsLog
{
    public class HabitLogEndpoints
    {
        private readonly HabitService _habitsService = new HabitService();
        private readonly HabitLogService _habitsLogService = new HabitLogService();
        private readonly HabitCreation _habitCreation;
        private readonly HabitLogInputs _habitInputs = new HabitLogInputs(TimeProvider.System);
        private readonly HabitLogModification _habitsModification = new HabitLogModification();
        public HabitLogEndpoints()
        {
            _habitCreation = new HabitCreation(TimeProvider.System);
        }
        public async Task Create()
        {
            ConsoleUtils.WriteTitle("LOG CREATION");
            Console.WriteLine("YOUR ACTUAL HABITS: ");

            ICollection<HabitModel> habits = await _habitsService.GetAll();
            Console.WriteLine("{0,-5}| {1,-20}", "Id", "Name");

            foreach (var habit in habits)
            {
                Console.WriteLine("{0,-5}| {1,-20}", habit.Id, habit.Name);
            }

            ConsoleUtils.WriteBorder();

            PostHabitLogModel newHabit = await _habitCreation.CreateHabit();
            await _habitsLogService.Create(newHabit);

            ConsoleUtils.WriteSuccess("Your log has been added");
        }
        public async Task GetAll()
        {
            ICollection<HabitLogModel> habits = await _habitsLogService.GetAll();

            if (habits.Count > 0)
            {
                ConsoleUtils.WriteBorder();
                Console.WriteLine("{0,-5}| {1,-20}| {2,-10}| {3,-10}", "Id", "Name", "Amount", "Date");
                ConsoleUtils.WriteBorder();

                foreach (HabitLogModel habit in habits)
                {
                    DateTimeOffset datetime = DateTimeOffset.FromUnixTimeSeconds(habit.UnixTimeStamp).ToLocalTime();

                    Console.WriteLine("{0,-5}| {1,-20}| {2,-10}| {3,-10:d}",
                        habit.Id, habit.HabitName, habit.Amount, datetime);
                }
            } else
            {
                Console.WriteLine("You don't have any logs created");
            }
        }
        public async Task Modify()
        {
            ConsoleUtils.WriteTitle("LOG MODIFICATION");

            int idHabit = await _habitInputs.GetHabitLogId("Enter the id of the log that you want modify: ");

            HabitLogModel? oldLog = await _habitsLogService.Get(idHabit);
            HabitLogModel modifiedLog = _habitsModification.ModifyHabit(oldLog);

            await _habitsLogService.Modify(modifiedLog);

            ConsoleUtils.WriteSuccess("The habit has been modified!");
        }

        public async Task Delete()
        {
            ConsoleUtils.WriteTitle("TASK DELETION");

            int idHabit = await _habitInputs.GetHabitLogId("Enter the id of the log that you want delete: ");
            Console.Write($"Do you want to delete log #{idHabit}? (y/n): ");
            string confirmation = Console.ReadLine() ?? String.Empty;

            if (String.Equals(confirmation, "y", StringComparison.CurrentCultureIgnoreCase))
            {
                await _habitsLogService.Delete(idHabit);
                ConsoleUtils.WriteSuccess("The log has been deleted!");
            }
        }
    }
}
