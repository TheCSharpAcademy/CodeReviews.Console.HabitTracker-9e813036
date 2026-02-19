using HabitTracker.CSamu7.Habits;
using HabitTracker.CSamu7.Validations;

namespace HabitTracker.CSamu7
{
    public class DateCreation
    {
        private TimeProvider _timeProvider;
        public DateCreation(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public DateTimeOffset GetDate()
        {
            while (true)
            {
                Console.Write("Enter the date (yyyy/mm/dd). (Type today to enter the actual date): ");
                string date = Console.ReadLine() ?? "";

                if (date.ToLower().Trim() == "today")
                    return _timeProvider.GetLocalNow();

                DateValidation validation = new DateValidation(_timeProvider);
                var result = validation.Validate(date);

                if (result.IsValid)
                {
                    return _timeProvider.GetLocalNow();
                } else
                {
                    DisplayErrors(result);
                }
            }
        }
        private void DisplayErrors(ValidationResult<DateTimeOffset> result)
        {
            ConsoleUtils.WriteLine("Invalid Date", ConsoleColor.Red);

            foreach (var error in result.Errors)
            {
                ConsoleUtils.WriteLine($"* {error}", ConsoleColor.Red);
            }
        }
    }
}
