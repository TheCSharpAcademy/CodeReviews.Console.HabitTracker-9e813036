using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.stevenwardlaw
{
    internal class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";

        static void Main(string[] args)
        {
            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS hours_coding (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Date TEXT,
                                            Quantity INTEGER
                                            )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput();
        }

        // Method that loops through the main menu and the options available until the application is closed.
        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to close the application.");
                Console.WriteLine("Type 1 to View all records");
                Console.WriteLine("Type 2 to Insert a record");
                Console.WriteLine("Type 3 to Delete a record");
                Console.WriteLine("Typr 4 to Update a record");
                Console.WriteLine("----------------------------------\n");

                string commandInput = Console.ReadLine();

                /* Depending on the input entered by the user, call the relevant method,
                close the app by breaking out of the loop or display an error message
                if the input was invalid.*/
                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nExiting app now!\n");
                        closeApp = true;
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
                        Console.WriteLine("\nInvalid command. Please type a number from 1 to 4.\n");
                        break;
                }
            }
        }

        private static void Update()
        {
            string sql = "UPDATE hours_coding SET Date = @date, Quantity = @quantity WHERE id = @id";
            Console.Clear();
            // Display all of the records in the table so the user can easily put the ID
            GetAllRecords();
            Console.WriteLine("Please enter the ID of the record you want to update:");
            int id = Convert.ToInt32(Console.ReadLine());
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert the number of hours spent coding to replace the existing amount (no decimals allowed)\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = sql;
                tableCmd.Parameters.AddWithValue("@id", id);
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                int rowsUpdated = tableCmd.ExecuteNonQuery();
                connection.Close();

                // If a record was updated, display this to the user - otherwise display that no records were found.
                if (rowsUpdated > 0)
                {
                    Console.WriteLine($"{rowsUpdated} row has been updated.");
                }
                else Console.WriteLine("No record was found with this ID, no records have been updated.");
            }
        }

        private static void Delete()
        {
            string sql = "DELETE FROM hours_coding WHERE id = @id";
            Console.Clear();
            // Display all of the records in the table so the user can easily put the ID
            GetAllRecords();
            Console.WriteLine("Please enter the ID of the record you want to delete:");
            int id = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = sql;
                tableCmd.Parameters.AddWithValue("@id", id);
                int rowsDeleted = tableCmd.ExecuteNonQuery();
                connection.Close();

                // If a record was deleted, display this to the user - otherwise display that no records were found.
                if (rowsDeleted > 0)
                {
                    Console.WriteLine($"{rowsDeleted} row has been deleted");
                }
                else Console.WriteLine("No record was found with this ID, no records have been deleted.");
            }
        }

        private static void Insert()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert the number of hours spent coding (no decimals allowed)\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO hours_coding(date, quantity) VALUES('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        // Method to display all the records to the user
        private static void GetAllRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM hours_coding";
                var reader = tableCmd.ExecuteReader();

                // If there are rows returned, write each of these to the console, aligned neatly
                if (reader.HasRows)
                {
                    Console.WriteLine("{0,-5} {1,-10} {2,-5}", "ID", "Date", "Hours");

                    while (reader.Read())
                    {
                        var id = reader.GetInt16(0);
                        var date = reader.GetString(1);
                        var quantity = reader.GetString(2);

                        Console.WriteLine("{0,-5} {1,-10} {2,-5}", id, date, quantity);
                    }
                }
                else Console.WriteLine("\nTable has no records.\n");

                connection.Close();
            }

            
        }

        // Method to get a date from the user, allowing them to enter "0" to reurn to the main menu
        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease enter the date in the following format: dd-mm-yy. Type 1 to use today's date. Type 0 to return to the main menu");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();
            else if (dateInput == "1")
            {
                dateInput = DateTime.Today.ToString("dd-MM-yy");
            }
            else
            {
                while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datevalue))
                {
                    Console.WriteLine("That is not a date in the correct format. Please enter another date.");
                    dateInput = Console.ReadLine();
                }
            }

            return dateInput;
        }

        // Method to get a number input from the user, allowing them to enter "0" to return to the main menu
        internal static int GetNumberInput(string input)
        {
            Console.WriteLine(input);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}
