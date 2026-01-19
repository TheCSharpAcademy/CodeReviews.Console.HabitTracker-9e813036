namespace HabitLoggerApi;

public static class UserInterface
{
  public static void Run()
  {
    bool programDone = false;
    while (programDone == false)
    {
      Console.WriteLine("=== WELCOME TO HABIT LOGGER === ");
      Console.WriteLine("1. View all habits");
      Console.WriteLine("2. Add habit");
      Console.WriteLine("3. Update habit");
      Console.WriteLine("4. Delete a habit");
      Console.WriteLine("5. Exit");

      Console.Write("\nChoice: ");
      string? choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          break;
        case "2":
          break;
        case "3":
          break;
        case "4":
          break;
        case "5":
          programDone = true;
          Console.WriteLine("\nExiting..");
          Environment.Exit(0);
          break;
      }

      Console.Clear();
    }
  }
}