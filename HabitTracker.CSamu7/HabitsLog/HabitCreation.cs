namespace HabitTracker.CSamu7.HabitsLog
{
    internal class HabitCreation
    {
        private TimeProvider _timeProvider;
        private readonly HabitLogInputs _inputs = new HabitLogInputs(TimeProvider.System);
        public HabitCreation(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public async Task<PostHabitLogModel> CreateHabit()
        {
            int idHabit = await _inputs.GetHabitId("Enter the id of the habit: ");
            DateTimeOffset date = _inputs.GetDate("Enter the date (yyyy/mm/dd). (Type today to enter the actual date): ");
            int amount = _inputs.GetAmount("Enter an amount: ");

            return new PostHabitLogModel(idHabit, amount, date.ToUnixTimeSeconds());
        }
    }
}
