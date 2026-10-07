using System.Globalization;

namespace HabitTracker;
using Console = System.Console;

// Class copy/pasted from the previous project "CodeReviews.Console.Calculator"
public static class AskUser
{
    public static async Task<T> Ask<T>(string[] optionTexts, bool speechRecognition) where T : IParsable<T>
    {
        if (speechRecognition)
        {
            return await AskUserWithSpeech.Ask<T>(optionTexts);
        }
        return Ask<T>(optionTexts);
    }

    private static T Ask<T>(string[] optionTexts) where T : IParsable<T>
    {
        bool isValidOption = true;
        T? result;
        do
        {
            WriteNotValidOption(isValidOption);
            WriteOption(optionTexts);
            
            isValidOption = T.TryParse(Console.ReadLine(), CultureInfo.CurrentCulture, out result);
        } while (!isValidOption);

        // isValidOption is true when result get correctly parsed so != null
        return result!;
    }
    
    public static async Task<T?> AskOptional<T>(string[] optionTexts, bool speechRecognition) where T : struct, IParsable<T>
    {
        if (speechRecognition)
        {
            return await AskUserWithSpeech.AskOptional<T>(optionTexts);
        }
        return AskOptional<T>(optionTexts);
    }
    // Optional input version (Returns null if user enters empty string)
    public static T? AskOptional<T>(string[] optionTexts) where T : struct, IParsable<T>
    {
        bool isValidOption = true;
        T? result = null;
        do
        {
            WriteNotValidOption(isValidOption);
            WriteOption(optionTexts);

            string? input = Console.ReadLine();

            // If empty or whitespace, treat as null (valid optional choice)
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            isValidOption = T.TryParse(input, CultureInfo.CurrentCulture, out var parsedResult);
            if (isValidOption)
            {
                result = parsedResult;
            }

        } while (!isValidOption);

        return result;
    }
    
    public static async Task<string?> AskOptional(string[] optionTexts, bool speechRecognition)
    {
        if (speechRecognition)
        {
            return await AskUserWithSpeech.AskOptional(optionTexts);
        }
        return AskOptional(optionTexts);
    }
    public static string? AskOptional(string[] optionTexts)
    {
        WriteOption(optionTexts);

        string? input = Console.ReadLine();

        // If the user hits Enter without typing anything, treat as null
        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }

    private static void WriteNotValidOption(bool isValid)
    {
        if (!isValid)
        {
            Console.WriteLine("Choose a correct option");
        }

    }
    public static void WriteOption(string[] optionTexts)
    {
        foreach (var optionText in optionTexts)
        {
            Console.WriteLine(optionText);
        }
    }

}