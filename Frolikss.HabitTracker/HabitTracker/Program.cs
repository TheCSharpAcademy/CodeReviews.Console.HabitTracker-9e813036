using System.Globalization;
using DatabaseLogic;

class Program
{
    private static DatabaseManager dbManager;
    private static string connectionString = "Data Source = habit-tracker.db";
    private static string dateFormat = "dd.MM.yyyy";

    static void Main()
    {
        var closeApp = false;
        dbManager = new DatabaseManager(connectionString);

        ShowMainMenu();

        while (!closeApp)
        {
            var input = Console.ReadLine();

            switch (input)
            {
                case "a":
                    AddNewRecord();
                    break;
                case "u":
                    UpdateRecord();
                    break;
                case "d":
                    DeleteRecord();
                    break;
                case "v":
                    ShowRecords();
                    break;
                case "q":
                    closeApp = true;
                    break;
                default:
                    Console.WriteLine("Incorrect input");
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("--- Main Menu ---");
        Console.WriteLine("Select an option: ");
        Console.WriteLine("a: add new record");
        Console.WriteLine("u: update record");
        Console.WriteLine("d: delete record");
        Console.WriteLine("v: view records");
        Console.WriteLine("q: quit");
    }

    static int GetHabitId()
    {
        Console.Clear();
        var habits = dbManager.GetAllHabits();
        var habitId = -1;

        var closeMenu = false;

        Console.WriteLine("Select an ID of existing habit or create a new one by entering 'n'");

        if (habits.Count > 0)
        {
            ShowHabits();
        }

        while (!closeMenu)
        {
            var input = Console.ReadLine();

            if (input == "n")
            {
                closeMenu = true;

                habitId = AddNewHabit();
                continue;
            }
            else if (habits.Count > 0 &&
                     int.TryParse(input, out var inputId) &&
                     habits.Any((habit) => habit.Id == inputId))
            {
                closeMenu = true;
                habitId = inputId;
            }
            else
            {
                Console.WriteLine("Incorrect input");
            }
        }

        return habitId;
    }

    static int AddNewHabit()
    {
        var habitId = -1;

        while (habitId <= 0)
        {
            Console.WriteLine("Write a habit name");
            var name = Console.ReadLine() ?? "";

            Console.WriteLine("Write units of measurement");
            var units = Console.ReadLine() ?? "";

            var habit = new Habit { Name = name, Units = units };
            habitId = dbManager.AddHabit(habit) ?? -1;

            if (habitId <= 0)
            {
                Console.WriteLine("Something went wrong, try creating a habit again");
            }
        }


        return habitId;
    }

    static void ShowHabits()
    {
        var habits = dbManager.GetAllHabits();

        Console.WriteLine(
            $"{"ID",-5} {"Name",-10} {"Units",-12}"
        );
        Console.WriteLine(
            $"{new string('-', 5)} {new string('-', 10)} {new string('-', 12)}"
        );

        habits.ForEach((record) =>
        {
            Console.WriteLine(
                $"{record.Id,-5} {record.Name,-10} {record.Units,-12}"
            );
        });
    }

    static void AddNewRecord()
    {
        var habitId = GetHabitId();

        Console.Clear();
        var record = GetRecordFromUser();
        record.Habit.Id = habitId;

        dbManager.AddRecord(record);
        Console.WriteLine("Record added successfully, enter any key to go back to main menu");
        Console.ReadLine();
        ShowMainMenu();
    }

    static void ShowRecords()
    {
        Console.Clear();
        var records = GetAndViewRecords();

        if (records.Count == 0)
        {
            ShowMainMenu();
            return;
        }
        
        Console.WriteLine("Press q to go back");

        var closeMenu = false;

        while (!closeMenu)
        {
            var input = Console.ReadLine();

            switch (input)
            {
                case "q":
                    closeMenu = true;
                    ShowMainMenu();
                    break;
                default:
                    Console.WriteLine("Incorrect input");
                    break;
            }
        }
    }

    static void UpdateRecord()
    {
        Console.Clear();
        var records = GetAndViewRecords();

        if (records.Count == 0)
        {
            ShowMainMenu();
            return;
        }

        Console.WriteLine("Select an ID of a record that you want to update");

        var closeMenu = false;

        while (!closeMenu)
        {
            var input = Console.ReadLine();

            if (int.TryParse(input, out var typedId) && records.Any((record) => record.Id == typedId))
            {
                closeMenu = true;

                var habitId = GetHabitId();
                var record = GetRecordFromUser();

                record.Id = typedId;
                record.Habit.Id = habitId;

                dbManager.UpdateRecord(record);
                Console.WriteLine("Record updated successfully, enter any key to go back to main menu");
                Console.ReadLine();
                ShowMainMenu();
            }
            else
            {
                Console.WriteLine("Incorrect input");
            }
        }
    }

    static void DeleteRecord()
    {
        Console.Clear();
        var records = GetAndViewRecords();

        if (records.Count == 0)
        {
            ShowMainMenu();
            return;
        }

        Console.WriteLine("Select an ID of a record that you want to delete");

        var closeMenu = false;

        while (!closeMenu)
        {
            var input = Console.ReadLine();

            if (int.TryParse(input, out var typedId) && records.Any((record) => record.Id == typedId))
            {
                closeMenu = true;

                dbManager.DeleteRecord(typedId);
                Console.WriteLine("Record deleted successfully, enter any key to go back to main menu");
                Console.ReadLine();
                ShowMainMenu();
            }
            else
            {
                Console.WriteLine("Incorrect input");
            }
        }
    }

    static List<Data> GetAndViewRecords()
    {
        var records = dbManager.GetAllRecords();

        if (records.Count == 0)
        {
            Console.WriteLine("No records");
            return new List<Data>();
        }

        Console.WriteLine(
            $"{"ID",-5} {"Quantity",-10} {"Date",-12} {"Habit",-15} {"Units",-10}"
        );
        Console.WriteLine(
            $"{new string('-', 5)} {new string('-', 10)} {new string('-', 12)} {new string('-', 15)} {new string('-', 10)}"
        );

        records.ForEach(record =>
        {
            Console.WriteLine(
                $"{record.Id,-5} {record.Quantity,-10} {record.Date?.ToString("dd.MM.yyyy"),-12} {record.Habit?.Name,-15} {record.Habit?.Units,-10}"
            );
        });

        return records;
    }

    static Data GetRecordFromUser()
    {
        var record = new Data();

        Console.WriteLine("Write a quantity of a habit");
        var quantity = GetQuantity();

        Console.WriteLine($"Write a date of habit in format {dateFormat}");
        var date = GetDate();

        record.Quantity = quantity;
        record.Date = date;
        return record;
    }

    static DateTime? GetDate()
    {
        DateTime? date = null;

        while (date is null)
        {
            var userDate = Console.ReadLine();
            if (DateTime.TryParseExact(
                    userDate,
                    dateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedDate))
            {
                date = parsedDate;
            }
            else
            {
                Console.WriteLine("Incorrect date");
            }
        }


        return date;
    }

    static int GetQuantity()
    {
        var quantity = -1;

        while (quantity <= 0)
        {
            var userQuantity = Console.ReadLine();

            if (int.TryParse(userQuantity, out var parsedQuantity) && parsedQuantity > 0)
            {
                quantity = parsedQuantity;
            }
            else
            {
                Console.WriteLine("Incorrect input");
            }
        }

        return quantity;
    }
}