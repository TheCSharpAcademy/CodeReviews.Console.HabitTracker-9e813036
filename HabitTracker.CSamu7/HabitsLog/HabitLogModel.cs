namespace HabitTracker.CSamu7.HabitsLog
{
    public record HabitLogModel(int Id, string HabitName, int Amount, long UnixTimeStamp);
    public record PostHabitLogModel(int IdHabit, int Amount, long UnixTimestamp);
    public record ModifyHabitLogModel(int Amount, long UnixTimeStamp);

    /* Sqlite doesn't have a date datatype, so I opted to use Unix Timestamp */
}
