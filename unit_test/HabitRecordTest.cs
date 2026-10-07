namespace HabitTracker.Tests;

public class HabitRecordTest
{
    [Fact]
    public void ConstructorWithSomeNullValue()
    {
        int? id = 123;
        Habit habit = new Habit("Water", "glasses", 1);
        int? quantity = null;
        DateTime? date = DateTime.Now;

        HabitRecord habitRecord = new HabitRecord(habit, id, quantity, date);
        
        Assert.Equal(habitRecord.Id, id);
        Assert.Equal(habitRecord.Habit, habit);
        Assert.Equal(habitRecord.Quantity, quantity);
        Assert.Equal(habitRecord.Date, date);
    }
    
    [Fact]
    public void ToStringMethodwithNullAndNotNullDate()
    {
        int id = 123;
        Habit habit = new Habit("Exercise", null, 10);
        int quantity = 321;
        HabitRecord habitRecord = new HabitRecord(habit, id, quantity, null);

        string habitString = $"Habit n°{id} : {habit.Name}, {quantity}{habit.Unit} - No date";
        Assert.Equal(habitRecord.ToString(), habitString);

        DateTime? date = DateTime.UnixEpoch;
        habitRecord.Date = date;
        string habitStringWithDate = $"Habit n°{id} : {habit.Name}, {quantity}{habit.Unit} - {date}";
        Assert.Equal(habitRecord.ToString(), habitStringWithDate);
    }
}