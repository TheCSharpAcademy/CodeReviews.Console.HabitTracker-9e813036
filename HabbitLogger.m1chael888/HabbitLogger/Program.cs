using Microsoft.Data.Sqlite;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Security.AccessControl;
//// m1chael888 \\\\
namespace HabbitLogger
{ 
    internal class Program
    {
        static void Main(string[] args)
        {
            string db = "Data Source=trackington.db";
            var habitList = new List<Habit>();
            var seenHabit = new List<string>();
            var habitOccurances = new List<Occurance>();

            InitializeDb();
            Menu();

            ////
            void Menu()
            {
                Console.Clear();
                Console.WriteLine(@"//// Trackington Menu \\\\");

                Console.WriteLine("\n-+-+-+-+-+-+-+-+-+-+-+-+-+-");
                ShowHabits();
                Console.WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-");

                Console.WriteLine("\na - Choose a habit to view or manage");
                Console.WriteLine("b - Create new habit to track");
                Console.WriteLine("c - Remove a habit and its records");
                Console.WriteLine("x - Close trackington");
                Console.Write("\nEnter the letter of your desired menu option: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    switch (input)
                    {
                        case "a":
                            if (habitList.Count == 0)
                            {
                                Red("No habits to view, choose another option: ");
                                input = Console.ReadLine();
                                break;
                            }
                            ChooseHabit();
                            done = true;
                            break;
                        case "b":
                            NewHabit();
                            break;
                        case "c":
                            if (habitList.Count == 0)
                            {
                                Red("No habits to delete, choose another option: ");
                                input = Console.ReadLine();
                                break;
                            }
                            DeleteHabit();
                            break;
                        case "x":
                            CloseApp();
                            break;
                        default:
                            Red("Please enter a valid menu option: ");
                            input = Console.ReadLine();
                            break;
                    }
                }
            }

            ////
            void ChosenHabit(int habit)
            {
                Console.Clear();
                Console.WriteLine(@$"//// {habitList.First(x => x.Id == habit).Unit} \\\\");

                Console.WriteLine("\n-+-+-+-+-+-+-+-+-+-+-+-+-+-");
                ShowOccurances(habit);
                Console.WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-");
                
                Console.WriteLine("\na - Insert a record");
                Console.WriteLine("b - Update a record");
                Console.WriteLine("c - Delete a record");
                Console.WriteLine("d - Return to menu");
                Console.WriteLine("x - Close Trackington");
                Console.Write("\nEnter the letter of your desired menu option: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    switch (input)
                    {
                        case "a":
                            InsertOccurance(habit);
                            done = true;
                            break;
                        case "b":
                            UpdateRecord(habit);
                            done = true;
                            break;
                        case "c":
                            DeleteRecord(habit);
                            done = true;
                            break;
                        case "d":
                            Menu();
                            done = true;
                            break;
                        case "x":
                            CloseApp();
                            break;
                        default:
                            Red("Please enter a menu number option: ");
                            input = Console.ReadLine();
                            break;
                    }
                }
            }

            ////
            void ChooseHabit()
            {
                Console.WriteLine("\n(Enter '0' to cancel)");
                Console.Write("Enter the number of the habit you would like to access: ");
                int num;
                string input = Console.ReadLine();

                if (input == "0") Menu();
                while (!int.TryParse(input, out num))
                {
                    Red("Please enter a valid habit number: ");
                }
                
                if (num == 0) Menu();
                else
                {
                    while (!habitList.Select(h => h.Id).Contains(num))
                    {
                        Red("Please enter a valid habit number: ");
                        int.TryParse(Console.ReadLine(), out num);
                    }
                    ChosenHabit(num);
                }
            }

            ////
            void ShowOccurances(int habit){
                habitOccurances.Clear();

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"SELECT * FROM Occurances WHERE HabitId=@habitid";
                    command.Parameters.Add("@habitid", SqliteType.Integer).Value = habit;

                    SqliteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            habitOccurances.Add(
                                new Occurance
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetString(1),
                                    Qty = reader.GetInt32(2)
                                });
                        }
                    }

                    if (habitOccurances.Count() > 0)
                    {
                        foreach (var o in habitOccurances)
                        {
                            Console.WriteLine($"Record {o.Id.ToString().PadRight(4)} -  {o.Qty}  times ({o.Date})");
                        }
                    }
                    else Console.WriteLine("No occurances found for this habit :(");
                }
            }

            ////
            void ShowHabits()
            {
                habitList.Clear(); seenHabit.Clear();
                populateHabitList();
                if (habitList.Count() > 0)
                {
                    foreach (var h in habitList)
                    {
                        if (!seenHabit.Contains(h.Id.ToString()))
                        {
                            seenHabit.Add(h.Id.ToString());
                            Console.WriteLine($"Habit {h.Id.ToString().PadRight(3)}-  {habitList.First(x => x.Id == h.Id).Unit}");
                        }
                    }
                }
                else Console.WriteLine("No habits found :(");
            }

            ////
            void InsertOccurance(int habit)
            {
                Console.WriteLine();
                Console.WriteLine("(Enter '0' to return to cancel)");
                string date = CaptureDate();
                int qty = CaptureQty();

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"INSERT INTO Occurances(Date, Qty, HabitId) VALUES(@date, @qty, @habitid)";
                    command.Parameters.Add("@date", SqliteType.Text).Value = date;
                    command.Parameters.Add("@qty", SqliteType.Integer).Value = qty;
                    command.Parameters.Add("@habitid", SqliteType.Integer).Value = habit;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                ChosenHabit(habit);
            }

            ////
            void UpdateRecord(int habit)
            {
                Console.WriteLine("\n(Enter '0' to cancel)");
                Console.Write("Enter the record number you would like to update: ");
                string input = Console.ReadLine();

                if (input == "0") ChosenHabit(habit);
                else
                {
                    while (!int.TryParse(input, out int Id) || !validRecordNum(Id))
                    {
                        Red($"Please enter a valid record number listed above: ");
                        input = Console.ReadLine();
                    }
                }
                                                                                                                                                                
                Console.WriteLine();
                string newDate = CaptureDate();
                int newQty = CaptureQty();

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"UPDATE Occurances SET Date = @newDate, Qty = @newQty WHERE OccId = @id";
                    command.Parameters.Add("@newDate", SqliteType.Text).Value = newDate;
                    command.Parameters.Add("@newQty", SqliteType.Integer).Value = newQty;
                    command.Parameters.Add("@id", SqliteType.Integer).Value = Convert.ToInt32(input);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Console.Clear();
                ChosenHabit(habit);
            }

            ////
            void DeleteRecord(int habit)
            {
                Console.WriteLine("\n(Enter '0' to cancel)");
                Console.Write("Enter the record number you would like to delete: ");
                string input = Console.ReadLine();

                if (input == "0") ChosenHabit(0);
                else
                {
                    while (!int.TryParse(input, out int Id) || !validRecordNum(Id))
                    {
                        Red($"Please enter a valid record number listed above: ");
                        input = Console.ReadLine();
                    }
                }

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"DELETE FROM Occurances WHERE OccId=@id";
                    command.Parameters.Add("@id", SqliteType.Integer).Value = Convert.ToInt32(input);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Console.Clear();
                ChosenHabit(habit);
            }

            ////
            void DeleteHabit()
            {
                Console.WriteLine("\n(Enter '0' to cancel)");
                Console.Write("Enter the number of the habit you would like to delete: ");
                string input = Console.ReadLine();

                if (input == "0") Menu();
                else
                {
                    while (!int.TryParse(input, out int Id) || !validHabitNum(Id))
                    {
                        Red($"Please enter a valid record number listed above: ");
                        input = Console.ReadLine();
                    }
                }

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = @"DELETE FROM Occurances WHERE HabitId=@id;
                                            DELETE FROM Habits WHERE HabitId=@id;";
                    command.Parameters.Add("@id", SqliteType.Integer).Value = Convert.ToInt32(input);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Console.Clear();
                Menu();
            }

            ////
            void NewHabit()
            {
                Console.WriteLine();
                Console.WriteLine("(Enter 0 to cancel)");

                string unit = CaptureUnit();

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"INSERT INTO Habits(Unit) VALUES(@unit)";
                    command.Parameters.Add("@unit", SqliteType.Text).Value = unit;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Menu();
            }

            ///
            int CaptureQty()
            {
                Console.Write("Enter the number of times you practiced your habit: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    if (input == "c") Menu();
                    else
                    {
                        if (!int.TryParse(input, out int qty))
                        {
                            Red("Please enter a whole number: ");
                            input = Console.ReadLine();
                        }
                        else done = true;
                    }
                }
                return Convert.ToInt32(input);
            }

            //////
            string CaptureDate()
            {
                Console.Write("What date did the habit occur? (MM/dd/yyyy format) You can enter 'today' to use todays date: ");
                string result = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    if (result == "0")
                    {
                        done = true;
                        Menu();
                    } 
                    else
                    {
                        if (result != "today" && !DateTime.TryParse(result, out DateTime date))
                        {
                            Red("Please enter a valid date (MM/dd/yyyy format) or 'today' to use today's date: ");
                            result = Console.ReadLine();
                        }
                        else done = true;
                    }
                }
                if (result.ToLower() == "today") result = DateTime.Today.ToString("MM/dd/yyyy");
                return result;
            }

            //////
            string CaptureUnit()
            {
                Console.Write("Enter your desired habit name/measurement (i.e. Times Hydrated, Pushups Done): ");
                string result = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    if (result == "0")
                    {
                        done = true;
                        Menu();
                    }
                    foreach (char c in result)
                    {
                        if (!(char.IsLetter(c) || (char.IsWhiteSpace(c))))
                        {
                            Red("Please only include letters and spaces: ");
                            result = Console.ReadLine();
                            break;
                        } else done = true;
                    }
                }
                return result;
            }

            ////
            bool validRecordNum(int id)
            {
                foreach (var h in habitOccurances)
                {
                    if (h.Id == id) return true;
                }
                return false;
            }

            ////
            bool validHabitNum(int id)
            {
                foreach (var h in habitList)
                {
                    if (h.Id == id) return true;
                }
                return false;
            }

            ////
            void Red(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"!!! {message}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            ////
            void CloseApp()
            {
                Console.WriteLine();
                Console.WriteLine(@"\\\\ Come again soon ////");
                Environment.Exit(0);
            }

            ////
            void ShouldSeed()
            {
                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"SELECT COUNT(HabitId) FROM Habits";

                    int rows = Convert.ToInt32(command.ExecuteScalar());
                    if (rows == 0) SeedDb();
                }
            }

            ////
            void populateHabitList()
            {
                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"SELECT * FROM Habits";

                    SqliteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            habitList.Add(
                                new Habit
                                {
                                    Id = reader.GetInt32(0),
                                    Unit = reader.GetString(1)
                                });
                        }
                    }
                }
            }

            ////
            void SeedDb()
            {
                Console.Write("Seeding database, please hold...");

                string[] seedHabits = 
                    { "Pushups done", "Book pages read", "Bottles of water drank", "Miles ran", "Plants watered", "Pullups done" };
                var seedOccurances = new List<List<string>>();
                var rand = new Random();

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();

                    foreach (var s in seedHabits)
                    {
                        var command = connection.CreateCommand();

                        command.CommandText = $"INSERT INTO Habits(Unit) VALUES(@unit)";
                        command.Parameters.Add("@unit", SqliteType.Text).Value = s;

                        command.ExecuteNonQuery();
                    }

                    populateHabitList();
                    for (int i = 0; i < 77; i++)
                    {
                        var command = connection.CreateCommand();

                        command.CommandText = $"INSERT INTO Occurances(Date, Qty, HabitId) VALUES(@date, @qty, @habitId)";

                        string date = "";
                        while (!DateTime.TryParse(date, out var date2))
                        {
                            date = $"{rand.Next(13)}/{rand.Next(32)}/{rand.Next(2020, 2027)}";
                        }
                        command.Parameters.Add("@date", SqliteType.Text).Value = date;
                        command.Parameters.Add("@qty", SqliteType.Integer).Value = rand.Next(1000);

                        int id;
                        if (habitList.Count() > 0) 
                        {
                            id = habitList[rand.Next(0, habitList.Count())].Id;
                        } 
                        else id = rand.Next(1, 7);
                        command.Parameters.Add("@habitId", SqliteType.Integer).Value = id;

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                Console.Clear();
            }

            ////
            void InitializeDb()
            {
                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS Habits(
                          HabitId INTEGER PRIMARY KEY AUTOINCREMENT,
                          Unit TEXT
                          );

                          CREATE TABLE IF NOT EXISTS Occurances(
                          OccId INTEGER PRIMARY KEY AUTOINCREMENT,
                          Date TEXT,
                          Qty INTEGER,
                          HabitId INTEGER,
                          FOREIGN KEY (HabitID) REFERENCES Habits(HabitID)
                          )";

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                ShouldSeed();
            }
        }
    }

    public class Habit
    {
        public int Id { get; set; }
        public required string Unit { get; set; }
    }

    public class Occurance
    {
        public int Id { get; set; }
        public required string Date { get; set; }
        public int Qty { get; set; }
    }
}
