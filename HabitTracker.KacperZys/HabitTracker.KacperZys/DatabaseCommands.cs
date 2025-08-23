using Microsoft.Data.Sqlite;

namespace HabitTracker.KacperZys;

internal class DatabaseCommands
{
    string connectionString = "Data Source=habitTracker.db";
    string habitName = "Hydration";
    List<string> rowTypes = new();
    List<string> rowNames = new();
    List<string> existingHabits = new();

    public DatabaseCommands()
    {
        string dbPath = "habitTracker.db";
        bool dbExists = File.Exists(dbPath);

        if (!dbExists) InitializeDefaultHabits();

        habitName = "Hydration";
        ReadTable();
    }

    private void InitializeDefaultHabits()
    {
        string query = @"
                        CREATE TABLE IF NOT EXISTS Hydration (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Quantity INTEGER,
                            DrinkType TEXT
                        )";

        DatabaseConnection(query);
        ReadTable();
        existingHabits.Add("Hydration");
        InsertRandomData("Hydration");

        query = @"
                CREATE TABLE IF NOT EXISTS Gym (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Hours INTEGER,
                        WorkoutType TEXT
                )";
        DatabaseConnection(query);
        existingHabits.Add("Gym");
        habitName = "Gym";
        ReadTable();
        InsertRandomData("Gym");

        void InsertRandomData(string habitName)
        {
            Random rnd = new();
            string date = string.Empty, randomText = string.Empty;
            int number = 0;

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < rowTypes.Count; j++)
                {
                    if (rowTypes[j] == "INTEGER")
                    {
                        number = rnd.Next(1, 6);
                    }
                    else
                    {
                        if (rowNames[j] == "Date")
                        {
                            int year = rnd.Next(1900, 2026);
                            int month = rnd.Next(1, 13);
                            int day = 0;

                            if (month == 2)
                            {
                                day = rnd.Next(1, 29);
                                date = new DateTime(year, month, day).ToString("yyyy-MM-dd");
                                continue;
                            }
                            else if (month == 4 || month == 6 || month == 9 || month == 11)
                            {
                                day = rnd.Next(1, 30);
                                date = new DateTime(year, month, day).ToString("yyyy-MM-dd");
                                continue;
                            }

                            day = rnd.Next(1, 31);
                            date = new DateTime(year, month, day).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            if (habitName == "Hydration")
                            {
                                randomText = Enum.GetName(typeof(DrinkType), rnd.Next(0, Enum.GetNames(typeof(DrinkType)).Length))!;
                                continue;
                            }
                            else if (habitName == "Gym")
                            {
                                randomText = Enum.GetName(typeof(WorkoutType), rnd.Next(0, Enum.GetNames(typeof(WorkoutType)).Length))!;
                                continue;
                            }
                        }
                    }
                }
                Insert(new List<string> { date, number.ToString(), randomText });
            }
        }
    }

    private void DatabaseConnection(string query, List<string>? input = null)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();
            table.CommandText = query;

            if (input != null)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    if (int.TryParse(input[i], out int result))
                    {
                        table.Parameters.AddWithValue($"@value{i}", result);
                    }
                    else

                    {
                        table.Parameters.AddWithValue($"@value{i}", input[i]);
                    }
                }
            }

            table.ExecuteNonQuery();
            connection.Close();
        }
    }
    private string GetDateInput()
    {
        Console.WriteLine("Enter the date (YYYY-MM-DD):");
        string dateInput = Console.ReadLine()!;
        DateTime parsedDate;
        while (!DateTime.TryParse(dateInput, out parsedDate))
        {
            Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format:");
            dateInput = Console.ReadLine()!;
        }
        return parsedDate.ToString("yyyy-MM-dd");
    }

    private void ReadTable()
    {
        rowNames.Clear();
        rowTypes.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();
            table.CommandText = $"PRAGMA table_info('{habitName}')";
            var reader = table.ExecuteReader();

            while (reader.Read())
            {
                string columnName = reader.GetString(1);
                string columnType = reader.GetString(2);

                if (columnName.ToLower() == "id") continue;
                rowNames.Add(columnName);
                rowTypes.Add(columnType);
            }

            connection.Close();
        }
    }

    private List<string> ReadTableUserInput()
    {
        string input = string.Empty;
        List<string> userInputs = new();

        for (int i = 0; i < rowTypes.Count; i++)
        {
            Console.WriteLine($"Please enter {rowNames[i]}:");

            if (rowTypes[i].ToUpper().Trim() == "INTEGER")
            {
                input = Console.ReadLine()!;
                if (int.TryParse(input, out _))
                {
                    userInputs.Add(input);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    i--;
                }
                continue;
            }

            Console.WriteLine("Do you want to insert a date? (Y/N)");
            char userInput;

            try
            {
                userInput = Convert.ToChar(Console.ReadLine()!.ToUpper());
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a valid character.");
                Console.Clear();
                return ReadTableUserInput();
            }

            if (userInput == 'Y')
            {
                input = GetDateInput();
                userInputs.Add(input);
                continue;
            }
            else if (userInput == 'N')
            {
                Console.WriteLine("You have chosen to insert a string.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter Y or N.");
                Console.Clear();
                i--;
                continue;
            }

            input = Console.ReadLine()!;

            if (CheckForStringValidation(input))
            {
                userInputs.Add(input);
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
                i--;
            }
        }
        Console.Clear();

        return userInputs;
    }

    private void ViewExistingHabits()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();
            table.CommandText = @"
                    SELECT name FROM sqlite_master
                    WHERE type='table' AND name NOT LIKE 'sqlite_%';";
            var reader = table.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
            }
            Console.WriteLine();
            connection.Close();
        }
    }

    public string ChooseHabit()
    {
        string connectionString = "Data Source=habitTracker.db";
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        Console.WriteLine("List of existing habits:");
        ViewExistingHabits();

        Console.WriteLine("Please enter the name of the habit you want to work with:");
        habitName = Console.ReadLine()!;
        CheckForStringValidation(habitName);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();
            table.CommandText = @"
                    SELECT COUNT(*) FROM sqlite_master
                    WHERE type='table' AND name=@habitName;
                    )";

            table.Parameters.AddWithValue("@habitName", habitName);
            long count = (long)table.ExecuteScalar()!;

            if (count > 0)
            {
                Console.Clear();
                Console.WriteLine($"You have chosen the habit: {habitName}");
                ReadTable();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Habit '{habitName}' does not exist. Please try again.");
                return ChooseHabit();
            }

            connection.Close();
        }

        return habitName;
    }
    public void Insert(List<string> userInput = null)
    {
        if (userInput == null)
        {
            userInput = ReadTableUserInput();
        }
        string query = $"INSERT INTO {habitName}(";

        for (int i = 0; i < rowNames.Count; i++)
        {
            query += $"{rowNames[i]}, ";
        }

        query = query.TrimEnd(',', ' ') + ") VALUES(";

        for (int i = 0; i < rowNames.Count; i++)
        {
            query += $"@value{i}, ";
        }

        query = query.TrimEnd(',', ' ') + ");";

        Console.Clear();
        DatabaseConnection(query, userInput);
    }

    public void Delete()
    {
        Console.WriteLine("Please provide an ID of a row you would like to delete or type \"all\" if you want to delete all data.");

        string IDToDelete = Console.ReadLine()!;

        if (IDToDelete.ToLower() == "all")
        {
            Console.WriteLine("You have chosen to delete all records.");
            string query = $"DELETE FROM {habitName}";
            DatabaseConnection(query);
            return;
        }

        if (string.IsNullOrEmpty(IDToDelete) || !int.TryParse(IDToDelete, out _))
        {
            Console.WriteLine("Invalid ID. Please enter a valid number.");
            return;
        }

        if (CheckIfExists(IDToDelete) != 0)
        {
            Console.Clear();
            Console.WriteLine($"Record with ID: {IDToDelete} will be deleted.");
            string query = $"DELETE FROM {habitName} WHERE Id = {IDToDelete}";
            DatabaseConnection(query);
        }
        else
        {
            return;
        }
    }
    public void Update()
    {
        Console.WriteLine("Please provide an ID you would like to modify:");
        string IDToUpdate = Console.ReadLine()!;

        if (string.IsNullOrEmpty(IDToUpdate) || !int.TryParse(IDToUpdate, out _))
        {
            Console.WriteLine("Invalid ID. Please enter a valid number.");
            return;
        }

        if (CheckIfExists(IDToUpdate) != 0)
        {
            Console.WriteLine($"Record with ID: {IDToUpdate} will be updated");
            List<string> userInput = ReadTableUserInput();
            string query = $"UPDATE {habitName} SET ";

            for (int i = 0; i < rowNames.Count; i++)
            {
                query += $"{rowNames[i]} = @value{i}, ";
            }

            query = query.TrimEnd(',', ' ') + $" WHERE Id = {IDToUpdate}";

            DatabaseConnection(query, userInput);
        }
    }
    public void View()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();
            string query = $"SELECT * FROM {habitName}";
            table.CommandText = query;

            var reader = table.ExecuteReader();
            Console.WriteLine();
            int i = 0;
            while (reader.Read())
            {
                Console.Write($"ID: {reader.GetInt32(i)} ");
                dynamic value;

                for (int j = 0; j < rowNames.Count; j++)
                {
                    i++;
                    if (rowTypes[j] == "INTEGER")
                    {
                        value = reader.GetInt32(i);
                    }
                    else
                    {
                        value = reader.GetString(i);
                    }

                    Console.Write($"{rowNames[j]}: {value} ");
                }

                i = 0;
                Console.WriteLine();
            }

            Console.WriteLine();
            connection.Close();
        }
    }

    private int CheckIfExists(string IDToDelete)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();
            string query = $"SELECT COUNT(*) FROM {habitName} WHERE Id = {IDToDelete}";
            table.CommandText = query;
            int count = Convert.ToInt32(table.ExecuteScalar());
            if (count == 0)
            {
                Console.WriteLine($"No record found with ID: {IDToDelete}");
                return 0;
            }
            return count;
        }
    }

    private bool CheckForStringValidation(string toValidate)
    {
        if (string.IsNullOrEmpty(toValidate))
        {
            Console.WriteLine("Input cannot be empty. Please try again.");
            return false;
        }
        else if (int.TryParse(toValidate, out _))
        {
            Console.WriteLine("Input cannot be a number. Please try again.");
            return false;
        }

        return true;
    }

    public void AddHabit()
    {
        Console.WriteLine("Please enter the name of your habit:");
        string habitName = Console.ReadLine()!;

        CheckForStringValidation(habitName);

        bool CheckToContinue = true;
        List<(string rowName, string rowType)> habitRows = new();

        while (CheckToContinue)
        {
            Console.WriteLine("Enter type of row (TEXT, INTEGER, DATE):");
            string rowType = Console.ReadLine()!.ToUpper();
            CheckForStringValidation(rowType);

            switch (rowType)
            {
                case "TEXT":
                    Console.WriteLine("You have chosen Text type row.");
                    break;
                case "INTEGER":
                    Console.WriteLine("You have chosen Number type row.");
                    break;
                case "DATE":
                    Console.WriteLine("You have chosen Date type row.");
                    break;
                default:
                    Console.WriteLine("Invalid row type. Please enter Text or Integer.");
                    return;
            }

            Console.WriteLine("Enter Name of the row:");
            string rowName = Console.ReadLine()!;
            CheckForStringValidation(rowName);

            habitRows.Add((rowName, rowType));

            Console.WriteLine("Do you want to add another row? (Y/N)");
            char userInput;
            try
            {
                userInput = Convert.ToChar(Console.ReadLine()!.ToUpper());
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a valid character.");
                Console.Clear();
                return;
            }

            if (userInput == 'Y')
            {
                continue;
            }
            else if (userInput == 'N')
            {
                CheckToContinue = false;
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter Y or N.");
                Console.Clear();
                return;
            }
        }

        string connectionString = "Data Source=habitTracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();

            table.CommandText = @$"
                    CREATE TABLE IF NOT EXISTS {habitName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ";

            string columns = string.Join(", ", habitRows.Select(r => $"{r.rowName} {r.rowType}"));

            table.CommandText += columns;
            table.CommandText = table.CommandText.TrimEnd(',', ' ') + ");";

            table.ExecuteNonQuery();
            connection.Close();
        }
    }

    private enum DrinkType
    {
        Water,
        Juice,
        Soda,
        Tea,
        Coffee
    }
    private enum WorkoutType
    {
        Cardio,
        Strength,
        Flexibility,
        Balance,
        HIIT
    }
}