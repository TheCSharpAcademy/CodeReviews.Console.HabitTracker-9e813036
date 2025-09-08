using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;


namespace HabbitLogger.Kris536b
{
    class Program
    {
        static string connectionString = @"Data Source=HabbitLogger.db";

        static void Main(string[] args)
        {

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    SqliteCommand tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS drinkingWater (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";

                    tableCmd.ExecuteNonQuery();
                }

                GetUserInput();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\nMAIN MENU");
                Console.WriteLine("Select an option:\n");
                Console.WriteLine("1. View All Records");
                Console.WriteLine("2. Insert Record");
                Console.WriteLine("3. Delete Record");
                Console.WriteLine("4. Update Record");
                Console.WriteLine("0. Exit Application.");
                Console.WriteLine("----------------------------------------");

                string? command = Console.ReadLine();

                switch (command?.Trim())
                {
                    case "0":
                        Console.WriteLine("Exiting application...");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    default:
                        Console.WriteLine("Invalid command. Type a number between 0 and 4.");
                        break;

                }
            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();

            try
            {
                List<DrinkingWater> tableData = new();

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    SqliteCommand tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = "SELECT * FROM drinkingWater";

                    using (SqliteDataReader reader = tableCmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string dateString = reader.GetString(1);

                                DateTime parsedDate;
                                bool validDate = DateTime.TryParseExact(dateString, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate);

                                if (!validDate)
                                {
                                    Console.WriteLine($"Warning: Could not parse date '{dateString}'. Setting as Unknown Date.");
                                    parsedDate = DateTime.MinValue;
                                }

                                DrinkingWater drinkingWater = new()
                                {
                                    Id = reader.GetInt32(0),
                                    Date = parsedDate,
                                    Quantity = reader.GetInt32(2)
                                };
                                tableData.Add(drinkingWater);
                            }
                        }

                        else
                        {
                            Console.WriteLine("No records found.");
                        }
                    }
                }

                Console.WriteLine("-----------------------------------------");
                foreach (var dw in tableData)
                {
                    string dateOutput = (dw.Date == DateTime.MinValue) ? "Unknown Date" : dw.Date.ToString("dd-MM-yy");

                    Console.WriteLine($"{dw.Id} - {dateOutput} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("-----------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving records: {ex.Message}");
                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void Insert()
        {
            string? date = GetDateInput();
            if (date == null)
            {
                return;
            }

            int? quantity = GetNumberInput("Insert number of glasses or other measure of your choice (no decimals allowed)");
            if (quantity == null)
            {
                return;
            }

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    SqliteCommand tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        $"INSERT INTO drinkingWater(date, quantity) VALUES('{date}', {quantity})";

                    tableCmd.ExecuteNonQuery();


                }

                Console.WriteLine("Record inserted successfully. Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the record: {ex.Message}");
                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            int? recordId = GetNumberInput("Type the Id of the record you want to delete or type '0' to return to main menu.");

            if (recordId == null)
            {
                return;
            }

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    SqliteCommand tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = $"DELETE from drinkingWater WHERE Id = {recordId}";

                    int rowCount = tableCmd.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"No record found with Id {recordId}. Press any key to return to main menu.");
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }
                }

                Console.WriteLine($"Record with Id {recordId} deleted successfully. Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the record: {ex.Message}");
                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        internal static void Update()
        {
            Console.Clear();
            GetAllRecords();

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    int? recordId;

                    while (true)
                    {
                        recordId = GetNumberInput("Type the Id of the record you want to update or type '0' to return to main menu.");
                        if (recordId == null)
                        {
                            return;
                        }

                        var checkCmd = connection.CreateCommand();
                        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinkingWater WHERE Id = {recordId})";
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (exists == 1)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"No record found with Id {recordId}. Try again or type '0' to return to main menu.");
                        }
                    }

                    string? date = GetDateInput();
                    if (date == null)
                    {
                        return;
                    }

                    int? quantity = GetNumberInput("Insert a new number of glasses or other measure of your choice (no decimals allowed)");
                    if (quantity == null)
                    {
                        return;
                    }

                    SqliteCommand tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"UPDATE drinkingWater SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

                    tableCmd.ExecuteNonQuery();

                    Console.WriteLine($"Record with Id {recordId} updated successfully. Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the record: {ex.Message}");
                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        internal static string? GetDateInput()
        {
            Console.WriteLine("Enter the date (dd-mm-yy) or type '0' to return to main menu.");

            string? dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                Console.Clear();
                return null;
            }

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date. Enter the date in dd-mm-yy format or type '0' to return to main menu.");
                dateInput = Console.ReadLine();
                if (dateInput == "0")
                {
                    Console.Clear();
                    return null;
                }
            }

            return dateInput;
        }

        internal static int? GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string? numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                Console.Clear();
                return null;
            }

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("Invalid input. Try again or type '0' to return to main menu.");
                numberInput = Console.ReadLine();

                if (numberInput == "0")
                {
                    Console.Clear();
                    return null;
                }
            }

            int finalQuantity = Convert.ToInt32(numberInput);

            return finalQuantity;
        }
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
