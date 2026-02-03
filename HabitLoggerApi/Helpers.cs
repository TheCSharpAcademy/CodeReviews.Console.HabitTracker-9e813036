using System.Globalization;

namespace HabitLoggerApi;

internal static class Helper
{
  internal static string? GetHabitName()
  {
    string? habitName = "";
    while (habitName?.Trim() == "")
    {
      Console.Write("Please insert the name. Type '0' to go back: ");
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

    int quantity = Convert.ToInt32(quantityInput);

    return quantity;
  }

  internal static string GetDate()
  {
    string? choice;

    do
    {
      Console.Write("Do you want to use the current date? [Y/N]: ");
      choice = Console.ReadLine()?.ToUpper().Trim();
    } while (choice != "Y" && choice != "N");


    if (choice == "Y")
      return DateOnly.FromDateTime(DateTime.Today).ToString();

    Console.Write("\nPlease provide the date in this format dd-MM-yy: ");
    string? dateInput = Console.ReadLine();

    DateTime date;

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out date))
    {
      Console.Write("Invalid date. (Format: dd-mm-yy). Try again: ");
      dateInput = Console.ReadLine();
    }

    return DateOnly.FromDateTime(date).ToString();
  }

  internal static int GetID(string option)
  {
    Console.Write($"Please provide the ID to be {option}. Type '0' to go back: ");
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