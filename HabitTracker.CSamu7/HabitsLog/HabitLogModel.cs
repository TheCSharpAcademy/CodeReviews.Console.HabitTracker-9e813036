namespace HabitTracker.CSamu7.HabitsLog
{
    public record HabitLogModel(int Id, int IdHabit, int Amount, DateOnly Date);
    public record PostHabitLogModel(int IdHabit, int Amount, DateOnly Date);
}
