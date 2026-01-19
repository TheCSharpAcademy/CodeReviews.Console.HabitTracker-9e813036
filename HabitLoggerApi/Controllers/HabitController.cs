using HabitLoggerApi.Models;

namespace HabitLoggerApi.Controllers;

internal static class HabitController
{
  internal static void AddHabit()
  {
    string? name = Helper.GetHabitName();
    int quantity = Helper.GetQuantityInput();
    string date = DateTime.Now.ToString();

    Database.CreateHabit(name, date, quantity);
  }

  internal static void ViewHabits()
  {
    List<Habit> allHabits = Database.QueryAllHabits();

    Console.WriteLine("ALL HABITS\n");

    if (allHabits.Count > 1)
    {
      foreach (var habit in allHabits)
      {
        Console.WriteLine(@$"ID: {habit.Id} - {habit.Name}, created at: {habit.Created_at} - Quantity: {habit.Quantity}");
      }
    }
    else
    {
      Console.WriteLine("No records yet.");
    }

    Console.Write("\nPress any key to continue..");
    Console.ReadKey();
  }
}