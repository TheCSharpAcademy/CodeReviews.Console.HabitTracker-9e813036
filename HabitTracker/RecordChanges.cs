using Microsoft.Data.Sqlite;
using System.Globalization;
public class RecordChanges
{
    static string connectionString = "Data Source = habitTracker.db";

    internal static void ViewRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText =
                "SELECT * FROM Coding";

            List<CodingHours> tableData = new();

            SqliteDataReader reader = tableCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //this is broken rn. need to fix
                    tableData.Add(
                    new CodingHours
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    }); ;
                }
            }
            else
            {
                Console.WriteLine("No rows are found");
            }
            connection.Close();

            Console.WriteLine("---------------------------------------------------------\n");

            foreach (var ch in tableData)
            {
                Console.WriteLine($"{ch.Id} - {ch.Date.ToString("mm-dd-yy")} - Hours: {ch.Quantity}");
            }

            Console.WriteLine("---------------------------------------------------------\n");
        }
    }
    internal static void AddRecord()
    {
        Console.Clear();
        string date = InputGetters.GetDateInput();
        int quantity = InputGetters.GetNumberInput("Please insert the number of hours you have coded today. Enter 0 (zero) to return to the main menu.");
        if (quantity <= 0)
        {
            Program.MainMenu();
        }
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText =
                $"INSERT INTO Coding(date, quantity) VALUES('{date}', '{quantity}')";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

    }

    internal static void DeleteRecord()
    {
        Console.Clear();
        ViewRecords();
        var recordid = InputGetters.GetNumberInput("Please type the id of the record you would like to delete, or 0 (zero) to return to the main menu.");
        if (recordid <= 0)
        {
            Program.MainMenu();
        }
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = $"DELETE from Coding WHERE id = '{recordid}'";

            int rowCount = tableCommand.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"Record with id: {recordid} does not exist.");
                DeleteRecord();
            }
            Console.WriteLine($"Record {recordid} was deleted.");

            connection.Close();

             Program.MainMenu();
        }
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        ViewRecords();

        var recordid = InputGetters.GetNumberInput("Please type the id of the record to update. Press 0 (zero) to return to the main menu.");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM Coding WHERE id = '{recordid}')";

            int query = Convert.ToInt32(tableCmd.ExecuteScalar());

            if (query == 0)
            {
                Console.WriteLine($"Record id {recordid} does not exist.");
                connection.Close();
                UpdateRecord();
            }

            string date = InputGetters.GetDateInput();
            int quantity = InputGetters.GetNumberInput("Please type how many hours you have coded today.");

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = $"UPDATE Coding SET date = '{date}', Quantity = '{quantity}' WHERE id = '{recordid}'";
            tableCommand.ExecuteNonQuery();
            connection.Close();

        }
    }

}