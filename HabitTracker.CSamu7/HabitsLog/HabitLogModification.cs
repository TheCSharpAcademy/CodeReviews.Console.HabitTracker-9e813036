namespace HabitTracker.CSamu7.HabitsLog
{
    public class HabitLogModification
    {
        private readonly HabitLogInputs _inputs = new HabitLogInputs(TimeProvider.System);
        public HabitLogModel ModifyHabit(HabitLogModel habit)
        {
            long unixTimeStamp = habit.UnixTimeStamp;
            Console.Write("Enter a new date? (y/n):  ");
            string inputNewDate = Console.ReadLine() ?? String.Empty;

            if (inputNewDate == "y")
            {
                DateTimeOffset newDate = _inputs.GetDate("Enter the new date (Enter today for the actual date) (yyyy/mm/dd): ");
                unixTimeStamp = newDate.ToUnixTimeSeconds();
            }

            int amount = habit.Amount;
            Console.Write("Enter a new amount? (y/n): ");
            string inputNewAmount = Console.ReadLine() ?? String.Empty;

            if (inputNewAmount == "y")
                amount = _inputs.GetAmount("Enter the new amount: ");

            return habit with { Amount = amount, UnixTimeStamp = unixTimeStamp };
        }
    }
}
