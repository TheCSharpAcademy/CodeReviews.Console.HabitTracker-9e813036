using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HabitTracker;

public enum Option 
{
    Insert,
    Delete,
    Update,
    View,
    SpeechToggle,
    Quit
}

public class UserOption: IParsable<UserOption>
{
    public Option? Option { get; set; }

    public UserOption(string input)
    {
        TryParse(input, CultureInfo.CurrentCulture, out var userOption);
        Option = userOption?.Option;
    }
    public UserOption(Option? option)
    {
        Option = option;
    }

    public static UserOption Parse(string? input, IFormatProvider? provider = null)
    {
        return TryParse(input, provider, out var result) ? result : throw new FormatException("Not a valid input");
    }

    public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? provider, [MaybeNullWhen(false)] out UserOption result)
    {
        result = new UserOption(input?.ToUpper().Trim() switch
            {
                "I" or "INSERT" => HabitTracker.Option.Insert,
                "D" or "DELETE" => HabitTracker.Option.Delete,
                "U" or "UPDATE" => HabitTracker.Option.Update,
                "V" or "VIEW" => HabitTracker.Option.View,
                "S" or "TOGGLE" => HabitTracker.Option.SpeechToggle,
                "Q" or "QUIT" => HabitTracker.Option.Quit,
                _ => null
            }
        );
        return (result.Option != null);
    }
}