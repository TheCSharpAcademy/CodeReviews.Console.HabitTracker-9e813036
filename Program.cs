using System.Data.SQLite;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitLogger;

public static class Validation
{
    public static bool IsValidColumnName(string? name)
    {
        string[] valid_names = ["name", "measurement", "quantity", "date"];
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        return valid_names.Contains(name.ToLower().Trim());
    }

    public static bool IsValidId(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Enter a valid ID");
            return false;
        }

        if (!int.TryParse(input, out int num))
        {
            Console.WriteLine("Enter a non-floating point number");
            return false;
        }
        if (num <= 0)
        {
            Console.WriteLine("ID must be greater than 0");
            return false;
        }

        return true;
    }

    public static bool IsValidMenuOption(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        return Regex.IsMatch(input, "^[0-4]$");
    }

    public static bool IsStringValid(string? name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    public static bool IsQuantityValid(string? quantity)
    {
        return double.TryParse(quantity, CultureInfo.InvariantCulture, out _);
    }

    public static bool IsDateValid(string? date, out DateOnly parsedDate)
    {
        parsedDate = default;
        if (string.IsNullOrWhiteSpace(date)) return false;

        if (date.ToLower().Equals("today"))
        {
            return true;
        }

        string[] formats = ["yyyy-MM-dd", "dd-MM-yyyy"];
        if (!DateOnly.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            return false;
        }

        if (parsedDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return false;
        }

        return true;
    }
}

class Program
{
    static readonly SQLiteConnection db = CreateDbConnection();
    static void Main(string[] args)
    {
        db.Open();

        string sql = "CREATE TABLE IF NOT EXISTS Habit(" +
                     "id_habit INTEGER PRIMARY KEY, " +
                     "name TEXT NOT NULL, " +
                     "measurement TEXT NOT NULL, " +
                     "quantity REAL NOT NULL, " +
                     "habit_date TEXT NOT NULL) ";
        ExecuteSqlNonQuery(sql, db);

        // Check if the table is empty and seed it if it is.
        string checkSql = "SELECT COUNT(*) FROM Habit";
        using (var checkCmd = new SQLiteCommand(checkSql, db))
        {
            long count = (long)checkCmd.ExecuteScalar();
            if (count == 0)
            {
                Console.WriteLine("Database is empty. Seeding with initial data...");
                SeedDatabase();
            }
        }

        string option;

        do
        {
            Console.Clear();
            ShowOptions();
            option = GetMenuInput();
            EvaluateUserOption(option);

            if (option != "0")
            {
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }

        } while (option != "0");

        db.Close();
    }

    static void SeedDatabase()
    {
        var habits = new List<(string Name, string Measurement)>
        {
            ("Running", "km"),
            ("Reading", "pages"),
            ("Coding", "hours"),
            ("Meditation", "minutes")
        };

        var random = new Random();
        string sql = "INSERT INTO Habit(name, measurement, quantity, habit_date) VALUES (@name, @measurement, @quantity, @date)";

        for (int i = 0; i < 100; i++)
        {
            var habit = habits[random.Next(habits.Count)];
            double quantity = Math.Round(random.NextDouble() * 10 + 1, 2); // Random quantity from 1.00 to 11.00
            DateOnly date = DateOnly.FromDateTime(DateTime.Today.AddDays(-random.Next(365))); // Random date in the last year

            using SQLiteCommand command = new(sql, db);
            command.Parameters.AddWithValue("@name", habit.Name);
            command.Parameters.AddWithValue("@measurement", habit.Measurement);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();
        }
    }

    static void ViewAllRecords()
    {
        string sql = "SELECT * FROM Habit";
        using SQLiteDataReader reader = GetReader(sql, db);
        ViewSelectedColumns(reader);
    }

    static void InsertHabit()
    {
        string habitName = GetValidName();
        string habitMeasurement = GetValidMeasurement();
        double habitQuantity = GetValidQuantity();
        string habitDate = GetValidDate();

        string sql = "INSERT INTO Habit(name, measurement, quantity, habit_date) VALUES (@name, @measurement, @quantity, @date)";
        using SQLiteCommand command = new(sql, db);
        command.Parameters.AddWithValue("@name", habitName);
        command.Parameters.AddWithValue("@measurement", habitMeasurement);
        command.Parameters.AddWithValue("@quantity", habitQuantity);
        command.Parameters.AddWithValue("@date", habitDate);

        command.ExecuteNonQuery();
        Console.WriteLine("\nRecord inserted successfully!");
    }

    static void DeleteHabit()
    {
        Console.Write("Enter the ID of the habit you wish to delete: ");

        int id = GetValidId();

        string sql = "DELETE FROM Habit WHERE id_habit = @id";
        try
        {
            using SQLiteCommand command = new(sql, db);
            command.Parameters.AddWithValue("@id", id);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine($"{rowsAffected} row was affected.");
            }
            else
                Console.WriteLine("Couldn't find a habit with that id.");

        }
        catch (SQLiteException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void UpdateHabit()
    {
        Console.WriteLine("Enter the ID of the habit you wish to update");
        int id = GetValidId();
        string column = GetValidColumnName();
        try
        {
            string sql;
            string valueParam;
            object value;

            switch (column.ToLower())
            {
                case "name":
                    value = GetValidName();
                    sql = "UPDATE Habit SET name = @value WHERE id_habit = @id;";
                    valueParam = "@value";
                    break;
                case "measurement":
                    value = GetValidMeasurement();
                    sql = "UPDATE Habit SET measurement = @value WHERE id_habit = @id;";
                    valueParam = "@value";
                    break;
                case "quantity":
                    value = GetValidQuantity();
                    sql = "UPDATE Habit SET quantity = @value WHERE id_habit = @id;";
                    valueParam = "@value";
                    break;
                case "date":
                    value = GetValidDate();
                    sql = "UPDATE Habit SET habit_date = @value WHERE id_habit = @id;";
                    valueParam = "@value";
                    break;
                default:
                    Console.WriteLine("Invalid column specified. Update aborted.");
                    return;
            }

            using SQLiteCommand command = new(sql, db);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue(valueParam, value);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("\nRecord updated successfully!");
            }
            else
            {
                Console.WriteLine("Couldn't find a habit with that ID.");
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }

    static string GetValidMeasurement()
    {
        Console.Write("Enter a unit of measurement (km, hours, books, movies): ");
        string? input = Console.ReadLine();

        while (!Validation.IsStringValid(input))
        {
            Console.Write("Enter a valid unit of measurement: ");
            input = Console.ReadLine();
        }
        return input!;
    }

    static string GetValidColumnName()
    {
        Console.WriteLine("Enter the field you wish to update");
        string? input = Console.ReadLine();
        while (!Validation.IsValidColumnName(input))
        {
            Console.WriteLine("Enter a valid column name (name, measurement, quantity, date)");
            input = Console.ReadLine();
        }
        return input!;
    }

    static string GetValidName()
    {
        Console.Write("Enter the habit name: ");
        string? input = Console.ReadLine();
        while (!Validation.IsStringValid(input))
        {
            Console.Write("Name cannot be empty. Please enter a valid name: ");
            input = Console.ReadLine();
        }
        return input!;
    }

    static double GetValidQuantity()
    {
        Console.Write("Enter the quantity: ");
        string? input = Console.ReadLine();
        while (!Validation.IsQuantityValid(input))
        {
            Console.Write("Invalid number. Please enter a valid quantity: ");
            input = Console.ReadLine();
        }
        return double.Parse(input!, CultureInfo.InvariantCulture);
    }

    static int GetValidId()
    {
        string? input = Console.ReadLine();
        while (!Validation.IsValidId(input))
        {
            input = Console.ReadLine();
        }
        return int.Parse(input!);
    }

    static string GetValidDate()
    {
        Console.Write("Enter the date (yyyy-MM-dd or dd-MM-yyyy) or type 'today' to insert today's date: ");
        string? input = Console.ReadLine();
        DateOnly parsedDate;

        while (!Validation.IsDateValid(input, out parsedDate))
        {
            Console.Write("Invalid date. Please use an accepted format and do not enter future dates: ");
            input = Console.ReadLine();
        }

        if (input!.ToLower().Equals("today"))
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            return today.ToString("yyyy-MM-dd");
        }

        return parsedDate.ToString("yyyy-MM-dd");
    }

    static void ViewSelectedColumns(SQLiteDataReader reader)
    {
        if (!reader.HasRows)
        {
            Console.WriteLine("No records found.");
            return;
        }

        Console.WriteLine("----------------------------------------------------------------");
        while (reader.Read())
        {
            int id = reader.GetInt32(reader.GetOrdinal("id_habit"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string measurement = reader.GetString(reader.GetOrdinal("measurement"));
            double quantity = reader.GetDouble(reader.GetOrdinal("quantity"));
            string date = reader.GetString(reader.GetOrdinal("habit_date"));
            DateTime dt = DateTime.Parse(date);

            Console.WriteLine($"Id: {id} | Habit: {name} | Quantity: {quantity} {measurement} | Date: {dt:d}");
        }
        Console.WriteLine("----------------------------------------------------------------");
    }

    static SQLiteDataReader GetReader(string selectSql, SQLiteConnection db)
    {
        SQLiteCommand command = new(selectSql, db);
        return command.ExecuteReader();
    }

    static void ExecuteSqlNonQuery(string sql, SQLiteConnection db)
    {
        using SQLiteCommand command = new(sql, db);
        command.ExecuteNonQuery();
    }

    static void EvaluateUserOption(string option)
    {
        switch (option)
        {
            case "0":
                Console.WriteLine("\nClosing application. Goodbye!");
                break;
            case "1":
                ViewAllRecords();
                break;
            case "2":
                InsertHabit();
                break;
            case "3":
                DeleteHabit();
                break;
            case "4":
                UpdateHabit();
                break;
        }
    }

    static void ShowOptions()
    {
        Console.WriteLine("MAIN MENU\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("Type 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("----------------------------\n");
        Console.Write("Your option? ");
    }

    static string GetMenuInput()
    {
        string? input = Console.ReadLine();
        while (!Validation.IsValidMenuOption(input))
        {
            Console.Write("Invalid option. Please choose a valid option (0-4): ");
            input = Console.ReadLine();
        }
        return input!;
    }

    static SQLiteConnection CreateDbConnection()
    {
        string dbPath = "HabitLogger.sqlite";
        string connectionString = $"Data Source={dbPath}";
        return new SQLiteConnection(connectionString);
    }
}