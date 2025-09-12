using System;

namespace error_messages
{
    public class DisplayError
    {
        public static void ErrorMessage(string error)
        {
            Console.WriteLine($"\n{error}\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}