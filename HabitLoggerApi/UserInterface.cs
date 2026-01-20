using HabitLoggerApi.Controllers;

namespace HabitLoggerApi;

public static class UserInterface
{
  public static void Run()
  {
    while (true)
    {
      Console.Clear();

      Console.WriteLine("=== WELCOME TO HABIT LOGGER === ");
      Console.WriteLine("1. View all habits");
      Console.WriteLine("2. Add habit");
      Console.WriteLine("3. Update habit");
      Console.WriteLine("4. Delete a habit");
      Console.WriteLine("5. Exit");

      Console.Write("\nChoice: ");
      string? choice = Console.ReadLine();

      Console.Clear();

      switch (choice)
      {
        case "1":
          HabitController.ShowAllHabits();
          break;
        case "2":
          HabitController.AddHabit();
          break;
        case "3":
          HabitController.UpdateHabitQuantity();
          break;
        case "4":
          break;
        case "5":
          Helper.ExitProgram();
          break;
      }
    }
  }
}