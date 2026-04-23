using Microsoft.Data.Sqlite;
using System.Globalization;
using static HabbitLogger.ValidationHelper;

string connectionString = @"Data Source=habit-logger.db";

CreateDB();

RunApp();

void RunApp()
{
    bool running = true;

    while (running)
    {
        DisplayMainMenu();

        string? userChoice = Console.ReadLine();

        switch (userChoice)
        {
            case "0":
                DisplayBasicMessage("Bye bye!");
                running = false;
                break;
            case "1":
                ViewHabit();
                DisplayBasicMessage("\n(Press Enter to return to main menu.)");
                break;
            case "2":
                AddHabit();
                break;
            case "3":
                DeleteHabit();
                break;
            case "4":
                UpdateHabit();
                break;
            default:
                DisplayBasicMessage("You've entered an incorrect option. Please try again. (Press Enter to continue)");
                break;
        }
    }
}

void DisplayMainMenu()
{
    Console.Clear();
    Console.WriteLine("Welcome to Habit Logger!");
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine("Please select one of the following options:");
    Console.WriteLine("\t- 0 to exit the application");
    Console.WriteLine("\t- 1 to view your logged habits");
    Console.WriteLine("\t- 2 to add a new habit log");
    Console.WriteLine("\t- 3 to delete a logged habit");
    Console.WriteLine("\t- 4 to update a logged habit");
}

void ViewHabit()
{
    try
    {
        List<(int id, string habitName, string date, int quantity, string unit)> habitsInfo = GetHabitsDB();
        

        Console.Clear();

        foreach (var habit in habitsInfo)
        {
            string tabulation = "\t";

            if (habit.habitName.Length <= 13)
                tabulation = "\t\t";

            if (habit.habitName.Length <= 5)
                tabulation = "\t\t\t";

            Console.WriteLine($"{habit.id}\t| {habit.habitName}{tabulation}| {habit.date}\t| {habit.quantity}\t| {habit.unit}");
        }

        Console.WriteLine("\n----------------------------------------------------------------------------------------------\n");
    }
    catch (SqliteException e)
    {
        DisplayErrorMessageDB(e, "view the habits");
    }
}

void AddHabit()
{
    string userHabit;
    string date;
    int quantity;
    string unit;

    ViewHabit();

    userHabit = GetHabitName();
    date = GetHabitDate();
    quantity = GetHabitQuantity();
    unit = GetHabitUnit();

    if (AddHabitToDB(userHabit, date, quantity, unit))
    {
        DisplayBasicMessage($"\n{userHabit} has been added to the Habbit Logger! (Press Enter to continue)");
    }
}

void DeleteHabit()
{
    int idToDelete;
    int numberOfRowsDeleted;

    while (true)
    {
        idToDelete = GetID();

        if (!ValidateID(idToDelete))
        {
            DisplayBasicMessage("\nPlease enter a valid ID. (Press Enter to continue)");
            continue;
        }

        numberOfRowsDeleted = DeleteHabitFromDB(idToDelete);

        if (!ValidateNumberOfRows(numberOfRowsDeleted))
        {
            DisplayBasicMessage("\nThe ID you entered doesn't exist. (Press Enter to continue)");
            continue;
        }

        DisplayBasicMessage($"\nThe Habit with id {idToDelete} has been successfully deleted! (Press Enter to continue)");
        break;
    }
}

void UpdateHabit()
{
    int idToUpdate;
    string newUserHabit;
    string newDate;
    int newQuantity;
    string newUnit;

    int numberOfRowsUpdated;

    while (true)
    {
        idToUpdate = GetID();

        if (!ValidateID(idToUpdate))
        {
            DisplayBasicMessage("\nPlease enter a valid ID. (Press Enter to continue)");
            continue;
        }

        newUserHabit = GetHabitName();
        newDate = GetHabitDate();
        newQuantity = GetHabitQuantity();
        newUnit = GetHabitUnit();

        numberOfRowsUpdated = UpdateHabitFromDB(idToUpdate, newUserHabit, newDate, newQuantity, newUnit);

        if (!ValidateNumberOfRows(numberOfRowsUpdated))
        {
            DisplayBasicMessage("\nThe ID you entered doesn't exist. (Press Enter to continue)");
            continue;
        }

        DisplayBasicMessage($"\nThe habit with ID {idToUpdate} has been updated successfully!");

        break;
    }
}

string GetHabitName()
{
    string? habitName;

    while (true)
    {
        Console.WriteLine("Please enter the name of the habit.");

        habitName = Console.ReadLine();

        if (!ValidateHabitTextData(habitName))
        {
            DisplayBasicMessage("\nPlease enter a valid habit name. (Press Enter to continue)");
            continue;
        }

        return habitName.Trim().ToLower();
    }

}

string GetHabitDate()
{
    string? userInput;
    DateTime habitDate;

    while (true)
    {
        Console.WriteLine("Please enter a date. (Format dd-MM-yyyy) (Press t to enter today's date)");
        userInput = Console.ReadLine();

        if (!ValidateHabitDate(userInput))
        {
            DisplayBasicMessage("\nPlease enter a valid date. (Press Enter to continue)");
            continue;
        }

        if (userInput.Trim().ToLower() == "t")
            return DateTime.Now.ToString("dd-MMM-yyyy");

        DateTime.TryParseExact(userInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out habitDate);
            
        return habitDate.ToString("dd-MMM-yyyy");
    }
}

int GetHabitQuantity()
{
    string? userInput;
    int habitQuantity;

    while (true)
    {
        Console.WriteLine("Please enter a quantity. (No decimals allowed)");
        userInput = Console.ReadLine();

        if (!ValidateHabitQuantity(userInput))
        {
            DisplayBasicMessage("\nPlease enter a valid quantity. (Press Enter to continue)");
            continue;
        }

        int.TryParse(userInput, out habitQuantity);

        return habitQuantity;
    }
}

string GetHabitUnit()
{
    string? habitUnit;

    while (true)
    {
        Console.WriteLine("Please enter the unit of the habit.");

        habitUnit = Console.ReadLine();

        if (!ValidateHabitTextData(habitUnit))
        {
            DisplayBasicMessage("\nPlease enter a valid habit unit. (Press Enter to continue)");
            continue;
        }

        return habitUnit.Trim().ToLower();
    }
}

int GetID()
{
    string? userInput;
    int habitID;

    while (true)
    {
        ViewHabit();

        Console.WriteLine("Please enter the ID of the habits you want to delete/update.");
        userInput = Console.ReadLine();

        if (userInput == null || !int.TryParse(userInput, out habitID))
        {
            habitID = -1;
        }

        return habitID;
    }
}

void CreateDB()
{
    bool fileExist = File.Exists("./habit-logger.db");
    try
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS habits(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            HABIT TEXT,
            DATE TEXT,
            QUANTITY INTEGER,
            UNIT TEXT
        )
        ";

        command.ExecuteNonQuery();

        if (!fileExist)
            GenerateRandomData();
    }
    catch (SqliteException e)
    {
        DisplayErrorMessageDB(e, "create the DB");
        Environment.Exit(0);
    }
}

void GenerateRandomData()
{
    var rand = new Random();
    
    List<string[]> habitList = new List<string[]> {
        new [] {"Running", "km" },
        new [] {"Walking", "steps"},
        new [] {"Reading", "pages"},
        new [] {"Drinking water", "glasses"},
        new [] {"Meditation", "minutes"},
        new [] {"Workout", "sessions"},
        new [] {"Cycling", "km"},
        new [] {"Coding Practice", "hours"},
        new [] {"Sleep", "hours"},
        new [] {"Calories intake", "kcal"},
        new [] {"Stretching", "minutes"},
        new [] {"Journaling", "entries"}
    };

    for (int i = 0; i < 100; i++)
    {
        string[] randomHabit = habitList[rand.Next(0, habitList.Count)];
        string randomHabitName = randomHabit[0].ToLower();
        string randomHabitUnit = randomHabit[1];
        string todayDate = DateTime.Now.ToString("dd-MMM-yyyy");
        int quantity = rand.Next(1, 11);

        AddHabitToDB(randomHabitName, todayDate, quantity, randomHabitUnit);
    }
}

bool AddHabitToDB(string userHabit, string date, int quantity, string unit)
{
    bool success = false;

    try
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = "INSERT INTO habits (HABIT, DATE, QUANTITY, UNIT) VALUES ($userHabit, $date, $quantity, $unit)";
        command.Parameters.AddWithValue("$userHabit", userHabit);
        command.Parameters.AddWithValue("$date", date);
        command.Parameters.AddWithValue("$quantity", quantity);
        command.Parameters.AddWithValue("$unit", unit);

        command.ExecuteNonQuery();

        success = true;
    }
    catch (SqliteException e)
    {
        DisplayErrorMessageDB(e, "add the habit");
    }

    return success;
}

int DeleteHabitFromDB(int idToDelete)
{
    int numberOfRowsDeleted = 0;

    try
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = "DELETE FROM habits WHERE ID = $idToDelete";
        command.Parameters.AddWithValue("$idToDelete", idToDelete);

        numberOfRowsDeleted = command.ExecuteNonQuery();
    }
    catch (SqliteException e)
    {
        DisplayErrorMessageDB(e, "delete the habit");
    }

    return numberOfRowsDeleted;
}

int UpdateHabitFromDB(int idToUpdate, string newUserHabit, string newDate, int newQuantity, string newUnit)
{
    int numberOfRowsUpdated = 0;
    try
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = "UPDATE habits SET HABIT = $newUserHabit, DATE = $newDate, QUANTITY = $newQuantity, UNIT = $newUnit WHERE ID = $idToUpdate";
        command.Parameters.AddWithValue("$newUserHabit", newUserHabit);
        command.Parameters.AddWithValue("$newDate", newDate);
        command.Parameters.AddWithValue("$newQuantity", newQuantity);
        command.Parameters.AddWithValue("$newUnit", newUnit);
        command.Parameters.AddWithValue("$idToUpdate", idToUpdate);

        numberOfRowsUpdated = command.ExecuteNonQuery();
    }
    catch (SqliteException e)
    {
        DisplayErrorMessageDB(e, "update the habit");
    }

    return numberOfRowsUpdated;
}

List<(int id, string habitName, string date, int quantity, string unit)> GetHabitsDB()
{
    List<(int id, string habitName, string date, int quantity, string unit)> habitsInfo = new();

    using var connection = new SqliteConnection(connectionString);

    connection.Open();

    using var command = connection.CreateCommand();

    command.CommandText = @"SELECT * FROM habits";

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        int id = reader.GetInt32(0);
        string habit = reader.GetString(1);
        string date = reader.GetString(2);
        int quantity = reader.GetInt32(3);
        string unit = reader.GetString(4);

        habitsInfo.Add((id, habit, date, quantity, unit));
    }

    return habitsInfo;
}

void DisplayErrorMessageDB(SqliteException e, string action)
{
    Console.WriteLine($"\nAn error occurred while trying to {action}. Please try again later.\nError: {e.Message}. (Press Enter to return to main menu)");
    Console.ReadLine();
}

void DisplayBasicMessage(string message)
{
    Console.WriteLine(message);
    Console.ReadLine();
}