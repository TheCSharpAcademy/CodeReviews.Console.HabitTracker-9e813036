using Microsoft.Data.Sqlite;

    public class Program
    {
        
        static string connectionString = "Data Source = habitTracker.db";
        static void Main(string[] args)
        {
            

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Coding(
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Date TEXT,
                                            Quantity INTEGER
                                            )";

                tableCommand.ExecuteNonQuery();
                connection.Close();
            }
            MainMenu();
        }
        internal static void MainMenu()
        {
            Console.Clear();
            bool close = false;
            while (!close)
            {
                Console.WriteLine("Welcome to the main menu");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. View records");
                Console.WriteLine("2. Add new record");
                Console.WriteLine("3. Delete an existing record");
                Console.WriteLine("4. Update an existing record");
                Console.WriteLine();
                string userInput = Console.ReadLine()!;

                switch (userInput)
                {
                    case "0":
                        Environment.Exit(0);
                        break;

                    case "1":
                        RecordChanges.ViewRecords();
                        break;
                    case "2":
                        RecordChanges.AddRecord();
                        break;
                    case "3":
                        RecordChanges.DeleteRecord();
                        break;
                    case "4":
                        RecordChanges.UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Selection not recognized. Please insert a number between 0 and 4.");
                        break;
                }
            }
        }
        
    }
    
    public class CodingHours
    {
        public int Id { get; set;}

        public DateTime Date{ get; set;}

        public int Quantity {get; set;}
    }
