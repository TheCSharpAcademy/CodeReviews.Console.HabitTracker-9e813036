using Habit_Tracker;

class Program
{
    static void Main(string[] args)
    {
        DatabaseManager.Start();
        if (DatabaseManager.GetHabits().Count == 0)
        {
            InitializeDatabase();
        }

        Console.WriteLine("Habit Tracker");
        bool endApp = false;
        while (!endApp)
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("\t1 - View habit list");
            Console.WriteLine("\t2 - View habit entries");
            Console.WriteLine("\t3 - View all information");
            Console.WriteLine("\t4 - Create new habit");
            Console.WriteLine("\t5 - Insert habit entry");
            Console.WriteLine("\t6 - Update habit entry");
            Console.WriteLine("\t7 - Delete habit");
            Console.WriteLine("\t8 - Delete habit entries");
            Console.WriteLine("\t0 - Close Application");

            string? option = Console.ReadLine();

            Console.WriteLine("--------------------------------\n");
            switch(option)
            {
                case "0":
                    endApp = true;
                    break;
                case "1":
                    ViewHabitList();
                    break;
                case "2":
                    ViewHabitEntries();
                    break;
                case "3":
                    ViewAllInfo();
                    break;
                case "4":
                    CreateHabit();
                    break;
                case "5":
                    InsertHabitEntry();
                    break;
                case "6":
                    UpdateHabitEntry();
                    break;
                case "7":
                    DeleteHabit();
                    break;
                case "8":
                    DeleteHabitEntries();
                    break;
                default:
                    Console.WriteLine("Choose a valid option");
                    break;
            }
            Console.WriteLine("");
        }
    }

    static void InitializeDatabase()
    {
        Random rng = new Random();
        DateOnly start = new(2020, 1, 1);
        DateOnly end = DateOnly.FromDateTime(DateTime.Now);
        int range = end.DayNumber - start.DayNumber;

        string[] habitNames = ["drink water", "brush teeth", "make bed"];
        int[] habitIds = new int[habitNames.Length];

        for (int i = 0; i < habitNames.Length; i++)
        {
            int id = DatabaseManager.CreateHabit(habitNames[i]);
            habitIds[i] = id;
        }

        int initialFillerAmount = 10;
        for(int i = 0; i < initialFillerAmount; i++)
        {
            DateOnly randomDate = start.AddDays(rng.Next(range + 1));
            DatabaseManager.InsertHabitEntry(habitIds[rng.Next(habitIds.Length)], $"{randomDate.ToString("yyyy-MM-dd")}",  rng.Next(1, 5));
        }
    }

    static void CreateHabit()
    {
        Console.WriteLine("");
        bool empty = true;
        while (empty)
        {
            Console.WriteLine("Write the name of the new habit you want to track:");
            string? habitName = Console.ReadLine();
            if (!string.IsNullOrEmpty(habitName))
            {
                try
                {
                    DatabaseManager.CreateHabit(habitName);
                    Console.WriteLine($"\nNew habit \"{habitName}\" created");
                    empty = false;
                }
                catch(Exception e)
                {
                    Console.WriteLine($"\nA habit named \"{habitName}\" already exists, try a different name");
                }
            }
        }
    }

    static void InsertHabitEntry()
    {
        List<(int Id, string Name)> habits = DatabaseManager.GetHabits();
        for (int i = 0; i < habits.Count; i++)
        {
            Console.WriteLine($"\t{i} - {habits[i].Name}");
        }
        int habitId = 0;
        string habitName = "";
        string entryDate = "";
        int entryQuantity = 0;
        bool validOption = false;
        while (!validOption)
        {
            Console.WriteLine("Choose a habit to record a new entry:");
            string? optionInput = Console.ReadLine();
            if (int.TryParse(optionInput, out int option))
            {
                if (option >= 0 && option < habits.Count)
                {
                    habitId = habits[option].Id;
                    habitName = habits[option].Name;
                    validOption = true;
                }
            }
        }
        validOption = false;
        while (!validOption)
        {
            Console.WriteLine("\nWrite the date in yyyy-MM-dd format, or type t to input today:");
            string? dateInput = Console.ReadLine();
            if (dateInput?.ToLower() == "t")
            {
                entryDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
                break;
            }
            if (DateOnly.TryParseExact(dateInput, "yyyy-MM-dd", out DateOnly date))
            {
                entryDate = date.ToString("yyyy-MM-dd");
                validOption = true;
            }
        }
        validOption = false;
        while (!validOption)
        {
            Console.WriteLine("\nWrite the number of occurrences:");
            string? quantityInput = Console.ReadLine();
            if (int.TryParse(quantityInput, out int quantity))
            {
                if (quantity > 0)
                {
                    entryQuantity = quantity;
                    validOption = true;
                }
            }
        }
        DatabaseManager.InsertHabitEntry(habitId, entryDate, entryQuantity);
        Console.WriteLine("Habit entry inserted:");
        Console.WriteLine($"\t{habitName}\t{entryDate}\t{entryQuantity}");
    }

    static void UpdateHabitEntry()
    {
        ShowHabitsAndChoose("Choose a habit to update:", out int habitId, out string habitName);
        List<(int Id, string Name, string Date, int Quantity)> habitEntries = ViewHabitEntries(habitId);
        bool validOption = false;
        int option = -1;
        string entryDate = "";
        int entryQuantity = -1;
        while (!validOption)
        {
            Console.WriteLine("Choose an entry to update:");
            string? optionInput = Console.ReadLine();
            if (int.TryParse(optionInput, out option))
            {
                if (option >= 0 && option < habitEntries.Count)
                {
                    validOption = true;
                }
            }
        }
        validOption = false;
        while (!validOption)
        {
            Console.WriteLine("Write new date, type t to input today or leave it empty to keep the previous value");
            string? dateInput = Console.ReadLine();
            if (string.IsNullOrEmpty(dateInput))
            {
                entryDate = habitEntries[option].Date;
                break;
            }
            if(dateInput.ToLower() == "t")
            {
                entryDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
                validOption = true;
            }
            if (DateOnly.TryParseExact(dateInput, "yyyy-MM-dd", out DateOnly date))
            {
                entryDate = date.ToString("yyyy-MM-dd");
                validOption = true;
            }
        }
        validOption = false;
        while (!validOption)
        {
            Console.WriteLine("\nWrite the new quantity, or leave it empty to keep the previous value:");
            string? quantityInput = Console.ReadLine();
            if (string.IsNullOrEmpty(quantityInput))
            {
                entryQuantity = habitEntries[option].Quantity;
                break;
            }
            if (int.TryParse(quantityInput, out int quantity))
            {
                if (quantity > 0)
                {
                    entryQuantity = quantity;
                    validOption = true;
                }
            }
        }
        DatabaseManager.UpdateHabitEntry(habitEntries[option].Id, entryDate, entryQuantity);
        Console.WriteLine("Habit entry updated");
    }

    static void DeleteHabit()
    {
        ShowHabitsAndChoose("Choose a habit to delete:", out int habitId, out string habitName);
        Console.WriteLine($"The habit \"{habitName}\" and all its entries will get deleted. Are you sure?");
        Console.WriteLine("y/n");
        string? confirmationInput = Console.ReadLine();
        if (confirmationInput?.ToLower() == "y")
        {
            DatabaseManager.DeleteHabit(habitId);
            Console.WriteLine($"Habit \"{habitName}\" and all its entries deleted");
        }
        else
        {
            Console.WriteLine("Deletion canceled");
        }
    }

    static void DeleteHabitEntries()
    {
        ShowHabitsAndChoose("Choose a habit to delete its entries:", out int habitId, out string habitName);
        List<(int Id, string Name, string Date, int Quantity)> habitEntries = ViewHabitEntries(habitId);
        if(habitEntries.Count == 0)
        {
            Console.WriteLine($"\"{habitName}\" has no entries to delete");
            return;
        }
        bool validOption = false;
        int option = -1;
        bool all = false;
        while (!validOption)
        {
            Console.WriteLine("Choose an entry to delete, or write \"all\" (without quotation marks) to delete all:");
            string? optionInput = Console.ReadLine();
            if (optionInput?.Trim().ToLower() == "all")
            {
                all = true;
                validOption = true;
            }
            if (int.TryParse(optionInput, out option))
            {
                if (option >= 0 && option < habitEntries.Count)
                {
                    validOption = true;
                }
            }
        }
        if (all)
        {
            Console.WriteLine($"All entries of habit \"{habitEntries[0].Name}\" will get deleted. Are you sure?");
            Console.WriteLine("y/n");
        }
        else
        {
            Console.WriteLine($"The entry {habitEntries[option].Id}-{habitEntries[option].Name}-{habitEntries[option].Date}-{habitEntries[option].Quantity} will get deleted. Are you sure?");
        }
        string? confirmationInput = Console.ReadLine();
        if (confirmationInput?.ToLower() == "y")
        {
            if (all)
            {
                DatabaseManager.DeleteEntriesOfHabit(habitId);
                Console.WriteLine($"All entries of habit \"{habitEntries[option].Name}\" deleted");
            }
            else
            {
                DatabaseManager.DeleteEntry(habitEntries[option].Id);
                Console.WriteLine($"Entry \"{habitEntries[option].Id}\" of habit \"{habitEntries[option].Name}\" deleted");
            }
        }
        else
        {
            Console.WriteLine("Deletion canceled");
        }
    }

    static void ShowHabitsAndChoose(string message, out int habitId, out string habitName)
    {
        Console.WriteLine("");
        List<(int Id, string Name)> habits = DatabaseManager.GetHabits();
        for (int i = 0; i < habits.Count; i++)
        {
            Console.WriteLine($"\t{i} - {habits[i].Name}");
        }
        habitId = 0;
        habitName = "";
        bool validOption = false;
        while (!validOption)
        {
            Console.WriteLine(message);
            string? optionInput = Console.ReadLine();
            if (int.TryParse(optionInput, out int option))
            {
                if (option >= 0 && option < habits.Count)
                {
                    habitId = habits[option].Id;
                    habitName = habits[option].Name;
                    validOption = true;
                }
            }
        }
    }

    static void ViewHabitList()
    {
        foreach (var habit in DatabaseManager.GetHabits())
        {
            Console.WriteLine($"\t{habit.Name}");
        }
    }

    static List<(int Id, string Name, string Date, int Quantity)> ViewHabitEntries(int habitId = 0)
    {
        Console.WriteLine("");
        List<(int Id, string Name, string Date, int Quantity)> habitList = DatabaseManager.GetHabitEntries(habitId);
        for (int i = 0; i < habitList.Count; i++)
        {
            Console.WriteLine($"\t{i} - \t{habitList[i].Name}\t{habitList[i].Date}\t{habitList[i].Quantity}");
        }
        return habitList;
    }

    static void ViewAllInfo()
    {
        foreach (var habit in DatabaseManager.GetHabitTotals())
        {
            Console.WriteLine($"\t{habit.Name}\tTotal: {habit.Total}");
        }
        ViewHabitEntries();
    }
}