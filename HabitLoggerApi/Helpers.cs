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
      Console.WriteLine("\n\nInvalid number. Try again.\n\n");
      quantityInput = Console.ReadLine();
    }

    Console.Clear();

    int quantity = Convert.ToInt32(quantityInput);

    return quantity;
  }
}