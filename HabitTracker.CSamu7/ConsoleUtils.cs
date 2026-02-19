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
                WriteError($"{numberString} is not a valid number");
                Console.Write("Enter a number again: ");
                numberString = Console.ReadLine() ?? string.Empty;
            }

            return number;
        }
        public static string ReadString()
        {
            string input = Console.ReadLine() ?? string.Empty;

            while (input == string.Empty)
            {
                WriteError($"{input} can't be an empty string");
                input = Console.ReadLine() ?? string.Empty;
            }

            return input;
        }
        public static void WriteLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void WriteError(string message)
        {
            WriteLine(message, ConsoleColor.Red);
        }
        public static void WaitKey()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
