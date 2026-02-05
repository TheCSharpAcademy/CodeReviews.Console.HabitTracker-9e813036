using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.CSamu7
{
    public class ConsoleUtils
    {
        public static int ReadInt()
        {
            string numberString = Console.ReadLine() ?? "";
            int number;

            while(!int.TryParse(numberString, out number))
            {
                Console.WriteLine($"{numberString} is not a valid number. Enter a number again");
                numberString = Console.ReadLine() ?? "";
            }

            return number;
        }
    }
}
