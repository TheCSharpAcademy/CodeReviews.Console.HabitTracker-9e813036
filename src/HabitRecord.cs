using System.Globalization;

namespace HabitTracker;

public class Habit
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string? Unit { get; set; }
    
    public Habit(string name, string? unit, int? id = null)
    {
        Id = id;
        Name = name;
        Unit = unit;
    }
}

public class HabitRecord
{
    public int? Id { get; set; }
    public Habit Habit { get; set; }
    public int? Quantity { get; set; }
    public DateTime? Date { get; set; }

    public HabitRecord(Habit habit, int? id = null, int? quantity = null, DateTime? date = null)
    {
        Id = id;
        Habit = habit;
        Quantity = quantity;
        Date = date; 
    }

    public override string ToString()
    {
        string formattedDate = Date?.ToString(CultureInfo.CurrentCulture) ?? "No date";
        return $"Habit n°{Id} : {Habit.Name}, {Quantity}{Habit.Unit} - {formattedDate}";
    }
}