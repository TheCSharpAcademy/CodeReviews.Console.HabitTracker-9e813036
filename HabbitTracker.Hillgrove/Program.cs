#region Setup
using System.Globalization;
using Microsoft.Data.Sqlite;

string connectionString = "Data Source=habbit-tracker.db";

using var connection = new SqliteConnection(connectionString);
connection.Open();

var command = connection.CreateCommand();
command.CommandText =
    @"
    CREATE TABLE IF NOT EXISTS daily_walk (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT NOT NULL,
        Minutes INTEGER NOT NULL,
        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
    );
";
command.ExecuteNonQuery();
#endregion

GetUserInput();

void GetUserInput()
{
    bool closeApp = false;
    while (!closeApp)
    {
        Console.Clear();
        Console.WriteLine("\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\n(0) to exit the app");
        Console.WriteLine("(1) to register a new walk");
        Console.WriteLine("(2) to view all walks");
        Console.WriteLine("(3) to update a walk");
        Console.WriteLine("(4) to delete a walk");
        Console.WriteLine("----------------------------------");

        Console.Write("\nEnter your choice: ");
        string? userInput = Console.ReadLine();

        switch (userInput)
        {
            case "0":
                closeApp = true;
                break;
            case "1":
                CreateWalk();
                break;
            case "2":
                ReadAllWalks();
                break;
            case "3":
                UpdateWalk();
                break;
            case "4":
                DeleteWalk();
                break;
            default:
                DisplayMessageAndWaitForKey("\nInvalid input. Press any key to try again.");
                break;
        }
    }
}

#region CRUD Methods
void CreateWalk()
{
    Console.Clear();

    string? date = GetDateInput();

    if (date == null)
        return;

    int quantity = GetNumberInput("\nEnter the number of minutes walked (no decimals allowed): ");
    if (quantity == -1)
        return;

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = "INSERT INTO daily_walk (Date, Minutes) VALUES (@date, @minutes)";
    command.Parameters.AddWithValue("@date", date);
    command.Parameters.AddWithValue("@minutes", quantity);

    command.ExecuteNonQuery();

    Console.WriteLine("\nWalk registered successfully");
    DisplayMessageAndWaitForKey();
}

void ReadAllWalks(bool clearConsole = true)
{
    if (clearConsole)
        Console.Clear();

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM daily_walk";

    SqliteDataReader reader = command.ExecuteReader();

    List<Walks> allWalks = [];

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            Walks walk = new Walks
            {
                Id = reader.GetInt32(0),
                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("da-DK")),
                Minutes = reader.GetInt32(2),
                CreatedAt = DateTime.Parse(reader.GetString(3)),
            };

            allWalks.Add(walk);
        }
    }
    else
    {
        Console.WriteLine("No walks found.");
    }

    Console.WriteLine("\n----------------------------------");
    foreach (Walks walk in allWalks)
    {
        Console.WriteLine($"Id: {walk.Id, 4} | Date: {walk.Date:dd-MM-yy} | Minutes: {walk.Minutes, 4} | Created At: {walk.CreatedAt}");
    }
    Console.WriteLine("----------------------------------");

    if (clearConsole)
        DisplayMessageAndWaitForKey();
}

void UpdateWalk()
{
    Console.Clear();
    ReadAllWalks(clearConsole: false);

    var id = GetNumberInput("\nEnter the Id of the walk you want to update\nType 0 to return to the main menu: ");

    if (id == -1)
        return;

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = "SELECT EXISTS(SELECT 1 FROM daily_walk WHERE Id = @id)";
    command.Parameters.AddWithValue("@id", id);

    bool walkExists = Convert.ToBoolean(command.ExecuteScalar());

    if (!walkExists)
    {
        Console.WriteLine($"\nNo walk found with the provided Id {id}");
        DisplayMessageAndWaitForKey();
        return;
    }

    string? newDate = GetDateInput();
    if (newDate == null)
        return;

    int newMinutes = GetNumberInput("\nEnter the new number of minutes walked (no decimals allowed): ");
    if (newMinutes == -1)
        return;

    command.CommandText = "UPDATE daily_walk SET Date = @date, Minutes = @minutes WHERE Id = @id";
    command.Parameters.Clear();
    command.Parameters.AddWithValue("@date", newDate);
    command.Parameters.AddWithValue("@minutes", newMinutes);
    command.Parameters.AddWithValue("@id", id);

    command.ExecuteNonQuery();
    Console.WriteLine($"\nWalk with Id {id} updated successfully");
    DisplayMessageAndWaitForKey();
}

void DeleteWalk()
{
    Console.Clear();
    ReadAllWalks(clearConsole: false);

    var id = GetNumberInput("\nEnter the Id of the walk you want to delete. Type 0 to return to the main menu: ");

    if (id == -1)
        return;

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = "DELETE FROM daily_walk WHERE Id = @id";
    command.Parameters.AddWithValue("@id", id);

    int rowsDeleted = command.ExecuteNonQuery();

    if (rowsDeleted == 0)
    {
        Console.WriteLine($"\nNo walk found with the provided Id {id}.");
    }
    else
    {
        Console.WriteLine($"\nWalk with Id {id} deleted successfully.");
    }

    DisplayMessageAndWaitForKey();
}
#endregion

#region Helper Methods
string? GetDateInput()
{
    while (true)
    {
        Console.Write("\nEnter the date of the walk (Format: dd-mm-yy)\nType 0 to return to the main menu: ");
        string? dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(dateInput))
        {
            Console.WriteLine("Date cannot be empty. Please try again.");
            continue;
        }

        if (DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("da-DK"), DateTimeStyles.None, out _))
        {
            return dateInput;
        }
        else
        {
            Console.WriteLine("Invalid date format. Please use dd-mm-yy format (e.g., 17-02-26).");
        }
    }
}

int GetNumberInput(string message)
{
    while (true)
    {
        Console.Write(message);

        string? numberInput = Console.ReadLine();

        if (numberInput == "0")
        {
            return -1;
        }

        if (string.IsNullOrWhiteSpace(numberInput))
        {
            Console.WriteLine("Input cannot be empty. Please try again.");
            continue;
        }

        if (int.TryParse(numberInput, out int finalQuantity))
        {
            if (finalQuantity > 0)
            {
                return finalQuantity;
            }
            else
            {
                Console.WriteLine("Please enter a positive number.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }
}
#endregion

void DisplayMessageAndWaitForKey(string message = "\nPress any key to return to the main menu")
{
    Console.Write(message);
    Console.ReadKey();
}

public class Walks
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Minutes { get; set; }
    public DateTime CreatedAt { get; set; }
}
