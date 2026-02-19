using HabitTracker.CSamu7.Validations;

namespace HabitTracker.CSamu7.HabitsLog
{
    internal class HabitCreation
    {
        private readonly DateCreation _dateCreation;
        private TimeProvider _timeProvider;
        public HabitCreation(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _dateCreation = new DateCreation(timeProvider);
        }
        public async Task<PostHabitLogModel> CreateHabit()
        {
            int idHabit = await GetIdHabit();
            DateTimeOffset date = _dateCreation.GetDate();
            int amount = GetAmount();

            return new PostHabitLogModel(idHabit, amount, date.ToUnixTimeSeconds());
        }
        private async Task<int> GetIdHabit()
        {
            IdHabitValidation validation = new IdHabitValidation();

            while (true)
            {
                Console.Write("Enter the id of the habit: ");

                int id = ConsoleUtils.ReadInt();
                var result = await validation.Validate(id);

                if (result.IsValid) return result.Value;

                ConsoleUtils.WriteError("This habit doesn't exist.");
            }
        }
        private int GetAmount()
        {
            var validation = new AmountValidation();

            while (true)
            {
                Console.Write("Enter an amount: ");
                int amount = ConsoleUtils.ReadInt();
                var result = validation.Validate(amount);

                if (result.IsValid) return result.Value;

                foreach (var error in result.Errors)
                {
                    ConsoleUtils.WriteError($"* {error}");
                }
            }
        }
    }
}
