namespace habitLogger._0lcm
{
    public class InputHelper
    {
        public static int GetIntInput(string? invalidInputPrompt = null, TextReader reader = null, int maxAttempts = 0)
        {
            reader ??= Console.In;
            invalidInputPrompt ??= "Please enter a number. Try again,";
            int currentAttempts = 0;

            while (true)
            {
                CheckIfMaxAttemptsReachedAndThrowError(maxAttempts, currentAttempts);

                string? userInput = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("String cannot be empty. Try again.");
                    currentAttempts++;
                    continue;
                } 

                if (int.TryParse(userInput, out int value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine(invalidInputPrompt);
                    currentAttempts++;
                    continue;
                }
            }
        }

        public static DateTime GetDateTimeInput(string? invalidInputPrompt = null, TextReader reader = null, int maxAttempts = 0)
        {
            reader ??= Console.In;
            invalidInputPrompt ??= "Please enter a convertible Date (d/m/y, or m/d/y). Try again.";
            int currentAttemps = 0;

            while (true)
            {
                CheckIfMaxAttemptsReachedAndThrowError(maxAttempts, currentAttemps);

                string? userInput = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Input cannot be empty. Try again.");
                    currentAttemps++;
                    continue;
                }
                
                if (userInput.ToLower().Trim() == "today")
                {
                    return DateTime.Today;
                }

                if (DateTime.TryParse(userInput, out DateTime value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine(invalidInputPrompt);
                    currentAttemps++;
                    continue;
                }
            }
        }

        public static T GetEnumInput<T>(
            Dictionary<string, T> shortcutMap,
            string? menu = null,
            string? invalidInputPrompt = null,
            TextReader reader = null,
            int maxAttempts = 0)
        {
            reader ??= Console.In;
            invalidInputPrompt ??= "This is not a valid input. Try again.";
            int currentAttempts = 0;

            if (!string.IsNullOrWhiteSpace(menu))
            {
                Console.WriteLine(menu);
            }

            while (true)
            {
                CheckIfMaxAttemptsReachedAndThrowError(maxAttempts, currentAttempts);

                string? userInput = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Input cannot be empty. Try again.");
                    currentAttempts++;
                    continue;
                }
                userInput = userInput.Trim();

                if (shortcutMap.TryGetValue(userInput, out T value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine(invalidInputPrompt);
                    currentAttempts++;
                    continue;
                }
            }
        }

        public static string GetStringInput(
            string? invalidInputPrompt = null,
            List<string>? validInputList = null,
            TextReader reader = null,
            int maxAttempts = 0)
        {
            invalidInputPrompt ??= "Invalid input. Please try again.";
            reader ??= Console.In;
            int currentAttempts = 0;

            while (true)
            {
                CheckIfMaxAttemptsReachedAndThrowError(maxAttempts, currentAttempts);

                string? userInput = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    currentAttempts++;
                    continue;
                }

                userInput = userInput.Trim();
                userInput = userInput.ToLower();
                if (validInputList ==  null || validInputList.Count == 0)
                {
                    return userInput;
                }
                else
                {
                    bool isValid = validInputList
                        .Where(v => v != null)
                        .Any(v => string.Equals(v, userInput, StringComparison.OrdinalIgnoreCase));

                    if (!isValid)
                    {
                        Console.WriteLine(invalidInputPrompt);
                        currentAttempts++;
                        continue;
                    }
                    return userInput;
                }
            }
        }

        private static void CheckIfMaxAttemptsReachedAndThrowError(int maxAttempts, int currentAtempts)
        {
            if (maxAttempts > 0 && currentAtempts >= maxAttempts)
            {
                throw new InvalidOperationException($"Operation terminated after {currentAtempts} attempts");
            }
        }
    }
}
