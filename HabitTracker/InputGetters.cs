using System.Globalization;

public class InputGetters
{
    internal static string GetDateInput()
    {
        Console.WriteLine("Enter the date (MM-dd-yy), or press 0 (zero) to return to the main menu: ");
        string dateInput = Console.ReadLine()!;

        if (dateInput == "0")
        {
            Program.MainMenu();
            return string.Empty;
        }

        bool inputValid = false;
        inputValid = ValidationHelper.ValidateDate(dateInput);

        while (!inputValid)
        {
            
            Console.WriteLine("Please enter an adequate date in the format MM-dd-YY");
            dateInput = Console.ReadLine()!;
            inputValid = ValidationHelper.ValidateDate(dateInput);
        }


        ;
        while (!DateTime.TryParseExact(dateInput, "MM-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid input. Please make sure the format is correct (MM-dd-yy). Press 0 (zero) to return to the main menu. ");
            dateInput = Console.ReadLine()!;
            inputValid = false;

            while (!inputValid)
            {
                inputValid = ValidationHelper.ValidateDate(dateInput);
                Console.WriteLine("Invalid input. Please make sure the format is correct (MM-dd-yy). Press 0 (zero) to return to the main menu. ");
                dateInput = Console.ReadLine()!;
            }
        }
        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string hours = Console.ReadLine()!;
        bool validation = false;
        validation = ValidationHelper.IsValidTime(hours);

        while (!validation)
        {
            hours = Console.ReadLine()!;
            validation = ValidationHelper.IsValidTime(hours);
        }

        if (hours == "0")
        {
            Program.MainMenu();
        }
        while (!Int32.TryParse(hours, out _) || Convert.ToInt32(hours) < 0)
        {
            Console.WriteLine("Invalid. Try again.");
            hours = Console.ReadLine()!;
        }


        return Convert.ToInt32(hours);
    }
}
