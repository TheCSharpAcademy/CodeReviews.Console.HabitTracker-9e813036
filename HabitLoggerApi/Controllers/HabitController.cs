using HabitLoggerApi.Models;

namespace HabitLoggerApi.Controllers;

internal static class HabitController
{
  internal static void AddHabit()
  {
    string? name = Helper.GetHabitName();

    if (name == "0")
      return;

    int quantity = Helper.GetQuantityInput();
    string date = Helper.GetDate();

    Database.CreateHabit(name, date, quantity);
  }

  internal static void ShowAllHabits()
  {
    RenderHabits();

    Console.Write("Press any key to continue..");
    Console.ReadKey();
  }

  internal static void UpdateHabit()
  {
    RenderHabits();

    if (Database.HabitRecordsLength() > 0)
    {
      int id = Helper.GetID();

      if (id == 0)
        return;

      string date = Helper.GetDate();
      int newQuantity = Helper.GetQuantityInput();
      bool success = Database.UpdateQuery(id, date, newQuantity);

      if (success)
        Console.WriteLine("\nSuccessfully updated!");
      else
        Console.WriteLine($"\nNo record found with ID of {id}.");
    }

    Console.Write("\nPress any key to continue..");
    Console.ReadKey();
  }

  internal static void DeleteHabit()
  {
    RenderHabits();

    if (Database.HabitRecordsLength() > 0)
    {

      int id = Helper.GetID();

      if (id == 0)
        return;

      string? choice;

      do
      {
        Console.Write("Are you sure? [Y/N]: ");
        choice = Console.ReadLine()?.ToUpper().Trim();
      } while (choice != "Y" && choice != "N");

      if (choice == "Y")
      {
        bool success = Database.DeleteQuery(id);

        if (success)
          Console.WriteLine("\nSuccessfully deleted!");
        else
          Console.WriteLine($"\nNo record found with ID of {id}.");
      }
    }

    Console.Write("\nPress any key to continue..");
    Console.ReadKey();
  }


  /* 
    Difference of ShowAllHabits and RenderHabits is RenderHabits is reusable
    while ShowAllHabits is just for rendering and having a 
    'menu option' quality with it 
  */
  static void RenderHabits()
  {
    List<Habit> allHabits = Database.QueryAllHabits();

    Console.WriteLine("-------------------------------------------------------------------------\n");

    if (allHabits.Count >= 1)
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

    Console.WriteLine("\n-------------------------------------------------------------------------\n");
  }
}