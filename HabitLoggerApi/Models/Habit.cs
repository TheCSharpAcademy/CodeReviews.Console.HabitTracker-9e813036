namespace HabitLoggerApi.Models;

public class Habit
{
  internal int Id { get; set; }
  internal string? Name { get; set; }
  internal int Quantity { get; set; }
  internal DateOnly Created_at { get; set; }
}