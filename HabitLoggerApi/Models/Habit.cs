namespace HabitLoggerApi.Models;

public class Habit
{
  internal int Id { get; set; }
  internal string? Name { get; set; }
  internal int Quantity { get; set; }
  internal DateTime Created_at { get; set; }
}