using HabitTracker.CSamu7.Utils;
using HabitTracker.CSamu7.Validations;

namespace HabitTracker.CSamu7.HabitsLog
{
    public class HabitLogInputs
    {
        private readonly TimeProvider _timeProvider;
        private readonly DateValidation _dateValidation;
        private readonly IdLogHabitValidation _logHabitValidation = new();
        private readonly AmountValidation _amountValidation = new();
        private readonly IdHabitValidation validation = new();
        public HabitLogInputs(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _dateValidation = new DateValidation(timeProvider);
        }
        public async Task<int> GetHabitLogId(string message)
        {
            while (true)
            {
                int id = ConsoleUtils.ReadInt(message);
                var result = await _logHabitValidation.Validate(id);

                if (result.IsValid) return result.Value;

                ConsoleUtils.WriteError(result.Error);
            }
        }
        public async Task<int> GetHabitId(string message)
        {
            while (true)
            {
                int id = ConsoleUtils.ReadInt(message);
                var result = await validation.Validate(id);

                if (result.IsValid) return result.Value;

                ConsoleUtils.WriteError(result.Error);
            }
        }
        public int GetAmount(string message)
        {
            while (true)
            {
                int amount = ConsoleUtils.ReadInt(message);
                var result = _amountValidation.Validate(amount);

                if (result.IsValid) return result.Value;

                ConsoleUtils.WriteError(result.Error);
            }
        }
        public DateTimeOffset GetDate(string message)
        {
            while (true)
            {
                Console.Write(message);
                string date = Console.ReadLine() ?? "";

                if (date.ToLower().Trim() == "today")
                    return _timeProvider.GetLocalNow();

                var result = _dateValidation.Validate(date);

                if (result.IsValid) return result.Value;

                ConsoleUtils.WriteError(result.Error);
            }
        }
    }
}
