namespace HabitTracker.CSamu7.HabitsLog
{
    public record HabitLogModel(int Id, int IdHabit, int Amount, DateOnly Date);
    public record PostHabitLogModel(int IdHabit, int Amount, long UnixTimestamp);
    /* Sqlite doesn't have a date datatype, so I opted to use Unix Timestamp */
}
