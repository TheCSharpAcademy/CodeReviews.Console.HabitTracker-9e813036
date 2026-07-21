using Spectre.Console;
using static HabitLogger.Enums;

namespace HabitLogger;

internal class HabitInterface
{
    internal bool MainMenu()
    {
        Console.Clear();

        var mainMenuOption = AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOptions>()
            .Title("Select which action you wish to do: ")
            .AddChoices(Enum.GetValues<MainMenuOptions>())
        );

        HabitController habitController = new();

        switch (mainMenuOption)
        {
            case MainMenuOptions.InsertRecord:
                habitController.InsertHabitLog();
                break;

            case MainMenuOptions.DeleteRecord:
                habitController.DeleteHabitLog();
                break;

            case MainMenuOptions.UpdateRecord:
                habitController.UpdateHabitLog();
                break;

            case MainMenuOptions.ViewAllRecords:
                habitController.ReadHabitLogs(onlyRead: true);
                break;

            default:
                return true;
        }

        return false;
    }

    internal (string?, string?, string?, string?) InsertUpdateHabitLogPrompt()
    {
        Console.Clear();

        Console.Write("Insert the quantity you achieved of this habit: ");
        string? quantity = Console.ReadLine();

        Console.Clear();

        var isToday = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Do you want to insert today's date?")
            .AddChoices(["Yes", "No"])
        );

        Console.Clear();

        if (isToday == "Yes")
        {
            return (quantity, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
        }
        else
        {
            Console.WriteLine("Please, insert the date this habit was done:");
            Console.Write("Year: ");
            string? year = Console.ReadLine();

            Console.Write("Month (1-12): ");
            string? month = Console.ReadLine();

            Console.Write("Day: ");
            string? day = Console.ReadLine();

            return (quantity, year, month, day);
        }
    }

    internal (string?, string?, string?) InsertNewHabitPrompt()
    {
        Console.Clear();

        Console.Write("Insert a name for this habit: ");
        string? name = Console.ReadLine();

        Console.Write("Insert a quantity goal for this habit (numeric value): ");
        string? quantityGoal = Console.ReadLine();

        Console.Write("Insert a unit this goal is going to be measured with: ");
        string? unit = Console.ReadLine();

        return (name, quantityGoal, unit);
    }

    internal int ShowHabitLogsTable(bool onlyRead, List<(int, string, double, bool, DateTime)> logs)
    {
        Console.Clear();

        int logToUpdate = 0;

        if (logs.Count > 0)
        {
            Table logsTable = new();

            logsTable.AddColumn("ID");
            logsTable.AddColumn("Name");
            logsTable.AddColumn("Quantity");
            logsTable.AddColumn("Goal");
            logsTable.AddColumn("Date");

            for (int i = 0; i < logs.Count; i++)
            {
                logsTable.AddRow((i + 1).ToString(), logs[i].Item2, logs[i].Item3.ToString("F2"), logs[i].Item4 ? "[green]OK[/]" : "[red]X[/]", logs[i].Item5.ToString("yyyy-MM-dd"));
            }

            AnsiConsole.Write(logsTable);
            Console.WriteLine();

            if (onlyRead == false)
            {
                do
                {
                    logToUpdate = AnsiConsole.Ask<int>("Choose an ID for the log you want to update [yellow](0 < x < quantityOfLogs)[/]: ");
                } while (logToUpdate <= 0 || logToUpdate > logs.Count);
            } else
            {
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }
        else
        {
            Console.WriteLine("There's no logs registered in this habit!");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        return logToUpdate - 1;
    }

    internal string SelectHabitPrompt(List<string> habits)
    {
        var habitSelected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select which habit type you wish to insert from")
            .AddChoices(habits)
        );

        return habitSelected;
    }

    internal void InvalidInputPrompt(string input)
    {
        Console.Clear();
        Console.WriteLine($"The inserted {input} is invalid, please try again.");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    internal void SuccessPrompt(string input)
    {
        Console.Clear();
        Console.WriteLine($"The habit log was {input} successfully!");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    internal void HabitAlreadyExistsPrompt(string input)
    {
        Console.Clear();
        Console.WriteLine($"The habit '{input}' already exists!");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
