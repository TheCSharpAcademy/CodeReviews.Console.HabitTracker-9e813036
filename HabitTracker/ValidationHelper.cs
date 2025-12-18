using System.Globalization;

public class ValidationHelper
{
    internal static bool IsValidTime(string hours)
    {
        int hourInt = Convert.ToInt32(hours);
        if (hourInt > 24)
        {
            Console.WriteLine("Invalid input. Hours coded in a day cannot exceed 24.");
            return false;
        }
        else if (hourInt < 0)
        {
            Console.WriteLine("Invalid input. Hours coded cannot be negative.");
            return false;
        }
        else
        {
            return true;
        }
    }

    internal static bool ValidateDate(string date)
    {
        var splitter = date.Split('-');

        int month = Convert.ToInt32(splitter[0]);
        int day = Convert.ToInt32(splitter[1]);
        int year = Convert.ToInt32(splitter[2]);

        if (month < 0 || month > 12)
        {
            Console.WriteLine("Invalid Month.");
            return false;
        }
        //this should account for the 31 day months, and negative/weird numbers
        if (day < 0 || day >= 32)
        {
            Console.WriteLine("Invalid. The day input is either below 0, or above 31. ");
            return false;
        }
        //febuary validation -- Not accounting for leap years. 
        if (month == 2)
        {
            if (day > 29)
            {
                Console.WriteLine("Invalid. Febuary only has a max of 29 days.");
                return false;
            }
        }
        //30 day months: april 4, june 6, sept 9, nov 11
        else if (month == 4 || month == 6 || month == 9 || month == 11)
        {
            if (day > 30)
            {
                Console.WriteLine("Invalid. The month you have used only has 30 days.");
                return false;
            }
        }
        //year validation 
        if (year < 0)
        {
            Console.WriteLine("Invalid. The year must be a non-negative integer.");
            return false;
        }

        return true;
    }
}