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
}