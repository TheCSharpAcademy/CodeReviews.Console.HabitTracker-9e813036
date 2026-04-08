using HabitTracker.Infrastructure;
using System.Globalization;

namespace HabitTracker.UI
{
    internal class InputService(SpeechService _speechService)
    {
        public async Task<int> GetNumberInputAsync(string message)
        {
            Console.WriteLine(message);
            string? numberInput = await GetInputAsync();

            while (numberInput != "0" && (!Int32.TryParse(numberInput, out int result) || result < 0))
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                numberInput = await GetInputAsync();
            }
            return Convert.ToInt32(numberInput);
        }

        public async Task<string> GetDateInputAsync()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-MM-yyyy). Type 0 to return.\n\n");
            string? dateInput = await GetInputAsync();

            while (dateInput != "0" && !DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid format. Use dd-MM-yyyy:\n\n");
                dateInput = await GetInputAsync();
            }
            return dateInput!;
        }

        public async Task<string?> GetInputAsync()
        {
            if (_speechService == null || !_speechService.IsEnabled)
                return Console.ReadLine();

            Console.WriteLine(" [Type your input, or press 's' + Enter to speak]");
            string? input = Console.ReadLine();

            if (input?.Trim().ToLower() == "s")
            {
                Console.WriteLine(" Listening...");

                string? recognized = await _speechService.RecognizeAsync();

                if (recognized != null)
                {
                    Console.WriteLine($" Recognized: {recognized}");
                    return recognized;
                }
                else
                {
                    Console.WriteLine(" Speech could not be recognized. Please type manually:");
                    return Console.ReadLine();
                }
            }
            return input;
        }
    }
}