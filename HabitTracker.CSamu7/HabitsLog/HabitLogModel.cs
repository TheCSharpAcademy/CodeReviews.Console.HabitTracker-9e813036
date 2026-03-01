namespace HabitTracker.CSamu7.HabitsLog
{
    /* Sqlite doesn't have a date data type, so I used Unix Timestamp */
    public record HabitLogModel(int Id, string HabitName, int Amount, long UnixTimeStamp);
    public record PostHabitLogModel(int IdHabit, int Amount, long UnixTimestamp);
    public record ModifyHabitLogModel(int Amount, long UnixTimeStamp);
}
