using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.CSamu7
{
    public class ConsoleUtils
    {
        public static int ReadInt()
        {
            string numberString = Console.ReadLine() ?? string.Empty;
            int number;

            while(!int.TryParse(numberString, out number))
            {
                Console.WriteLine($"{numberString} is not a valid number. Enter a number again");
                numberString = Console.ReadLine() ?? string.Empty;
            }

            return number;
        }

        public static string ReadString()
        {
            string input = Console.ReadLine() ?? string.Empty;

            while (input == string.Empty)
            {
                Console.WriteLine($"{input} can't be an empty string");
                input = Console.ReadLine() ?? string.Empty;
            }

            return input;
        }
    }
}
