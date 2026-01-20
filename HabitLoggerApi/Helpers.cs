namespace HabitLoggerApi;

internal static class Helper
{
  internal static string? GetHabitName()
  {
    string? habitName = "";
    while (habitName?.Trim() == "")
    {
      Console.Write("Please insert the name: ");
      habitName = Console.ReadLine();
    }

    return habitName;
  }

  internal static int GetQuantityInput()
  {

    Console.Write("Please insert the number of times you've done this (no decimals allowed): ");
    string? quantityInput = Console.ReadLine();

    while (!int.TryParse(quantityInput, out _) || Convert.ToInt32(quantityInput) < 0)
    {
      Console.WriteLine("Invalid number. Try again.");
      quantityInput = Console.ReadLine();
    }

    Console.Clear();

    int quantity = Convert.ToInt32(quantityInput);

    return quantity;
  }

  internal static int GetID()
  {
    Console.Write("Please provide the ID to be edited: ");
    string? idInput = Console.ReadLine();

    while (!int.TryParse(idInput, out _) || Convert.ToInt32(idInput) < 0)
    {
      Console.WriteLine("\nInvalid ID number. Try again.");
      idInput = Console.ReadLine();
    }

    int id = Convert.ToInt32(idInput);

    return id;
  }

  internal static void ExitProgram()
  {
    Console.WriteLine("Exiting..");

    Console.Write("\nPress any key to exit..");
    Console.ReadKey();

    Console.Clear();
    Environment.Exit(0);
  }
}