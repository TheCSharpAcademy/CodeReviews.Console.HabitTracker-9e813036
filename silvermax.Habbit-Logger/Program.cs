using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Globalization;


namespace Habbit_Logger
{
    internal class Program
    {
        static string connectionString = @"Data Source=habbit-logger.db";

        static void Main(string[] args)
        {
            Batteries.Init();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habbit_logger (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Habbit TEXT,
                                        Date TEXT,
                                        Tracker TEXT
                                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput();
        }

        static void GetUserInput()
        {
            bool stopApp = false;
            while (stopApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Habit Records.");
                Console.WriteLine("Type 2 to Insert a Habit.");
                Console.WriteLine("Type 3 to Delete a Habit Record.");
                Console.WriteLine("Type 4 to Update a Habit.");
                Console.WriteLine("--------------------------------------------------\n");

                string? commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye\n");
                        stopApp = true;
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
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        GetUserInput();
                        break;
                }
            }
        }

        private static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetRecordId("update");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM habbit_logger WHERE Id = @recordId)";

                checkCmd.Parameters.AddWithValue("@recordId", recordId);
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (checkQuery == 0 )
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    connection.Close();
                    Update();
                }

                var (habbit, tracker, date) = GetHabbit();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE habbit_logger SET Habbit = @habbit, Date = @date, Tracker = @tracker WHERE Id = @recordId";

                tableCmd.Parameters.AddWithValue("@habbit", habbit);
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@tracker", tracker);
                tableCmd.Parameters.AddWithValue("@recordId", recordId);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetRecordId("delete");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "DELETE FROM habbit_logger WHERE Id = @recordId";

                tableCmd.Parameters.AddWithValue("@recordId", recordId);

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Habbit Record with Id {recordId} doesn't exist.");
                    Delete();
                }

                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted\n\n");
            }

            GetUserInput();
        }

        private static void Insert()
        {
            var (habbit, tracker, date) = GetHabbit();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "INSERT INTO habbit_logger(Habbit, Date, Tracker) VALUES(@Habbit, @Date, @Tracker)";

                tableCmd.Parameters.AddWithValue("@Habbit", habbit);
                tableCmd.Parameters.AddWithValue("@Date", date);
                tableCmd.Parameters.AddWithValue("@Tracker", tracker);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT * FROM habbit_logger";

                List<Habbit> data = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        data.Add(new Habbit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("de-DE")),
                            Tracker = reader.GetString(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("-------------------------------------------");

                foreach (var info in data)
                {
                    Console.WriteLine($"{info.Id} - Habbit: {info.Name} - Date: {info.Date.ToString("dd-MMM-yyyy")} - Tracker: {info.Tracker}");
                }

                Console.WriteLine("--------------------------------------------");
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to maín menu");

            string? dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("de-DE"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to Main Menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        private static (string? habbit, string? tracker, string? date) GetHabbit()
        {
            string date = DateTime.UtcNow.ToString("dd-MM-yy"); //GetDateInput();

            Console.WriteLine("\n\nPlease input a habit that you want to add.\n\n");

            string? habbit = Console.ReadLine();

            Console.WriteLine("\n\nPlease input the tracking amount and quantity of the habit. For example 6 Pages for reading a book or 6 glasses for drinking Water.\n\n");

            string? tracker = Console.ReadLine();

            return (habbit, tracker, date);
        }

        private static int GetRecordId(string action)
        {
            Console.WriteLine($"Please enter the Id of the habit record you want to {action}.");

            var output = Console.ReadLine();

            var recordId = 0;

            while (!int.TryParse(output, out recordId))
            {
                Console.WriteLine("Please enter a valid recordId");
                output = Console.ReadLine();
            }

            return recordId;
        }
    }
}
