using HabitTracker.CSamu7.Habits;
using HabitTracker.CSamu7.Validations;
using System.Threading.Tasks;

namespace HabitTracker.CSamu7.HabitsLog
{
    public class HabitLogEndpoints
    {
        private TimeProvider _timeProvider;
        private readonly HabitService _habitsService = new HabitService();
        private readonly HabitLogService _habitsLogService = new HabitLogService();
        public HabitLogEndpoints(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public async Task CreateHabit()
        {
            //Display habits
            List<HabitModel> habits = await _habitsService.GetHabits();

            foreach(var habit in habits)
            {
                Console.WriteLine($"{habit.Id}. {habit.Name}");
            }

            Console.Write("Enter the id of the habit: ");

            int idHabit;

            while (int.TryParse(Console.ReadLine(), out idHabit))
            {
                Console.WriteLine("Enter an existent habit");
                //Si id no existe, volver a preguntar.
            }

            DateOnly date;

            while (true)
            {
                Console.Write("Enter the date. (Type today to enter the actual date)");
                string dateInput = Console.ReadLine() ?? "";

                if(dateInput.ToLower().Trim() == "today")
                {
                    DateTimeOffset today = _timeProvider.GetUtcNow();
                    date = new(today.Year, today.Month, today.Day);
                } else
                {
                    DateValidation validation = new DateValidation(_timeProvider);
                    var result = validation.Validate(dateInput);

                    if (result.IsValid)
                    {
                        date = result.Value;
                    } else
                    {
                        Console.WriteLine("Invalid Date");

                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"* {error}");
                        }
                    }
                }
            }

            Console.Write("Enter the amount: ");

            int amount;

            while(int.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Enter a valid amount");
            }

            _habitsLogService.CreateHabitLog(new PostHabitLogModel(idHabit, amount, date));


        }
    }
}
