using HabitTracker.Data;
using HabitTracker.Model;
using HabitTracker.UI;
using System.Globalization;

namespace HabitTracker.Application
{
    internal class HabitController(DatabaseManager databaseManager, InputService inputService)
    {
        public void GetAllRecords()
        {
            Console.Clear();

            List<HabitRecord> records = databaseManager.GetAllRecords();

            if (records.Count == 0)
                Console.WriteLine("No rows found");

            Console.WriteLine("------------------------------------------\n");
            foreach (var record in records)
            {
                Console.WriteLine($"{record.Id} - {record.HabitName} - {record.Date:dd-MMM-yyyy} - Quantity: {record.Quantity} {record.Unit}");
            }
            Console.WriteLine("------------------------------------------\n");
        }

        public async Task DeleteAsync()
        {
            Console.Clear();
            GetAllRecords();
            var recordId = await inputService.GetNumberInputAsync("\n\nPlease type the Id of the record you want to delete or 0 to go back\n\n");

            if (!databaseManager.DeleteRecord(recordId))
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                Console.WriteLine("Press any key to return to the Main Menu...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted.\n\n");
                Console.WriteLine("Press any key to return to the Main Menu...");
                Console.ReadLine();
            }
        }

        public async Task UpdateAsync()
        {
            GetAllRecords();
            var recordId = await inputService.GetNumberInputAsync("\n\nPlease type Id of the record you'd like to update. Type 0 to return.\n\n");

            if (!databaseManager.CheckRecordExists(recordId))
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                Console.WriteLine("Press any key to return to the Main Menu...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nEnter new habit name:");
            string name = await inputService.GetInputAsync() ?? "Unknown";

            Console.WriteLine("Enter new measurement unit:");
            string unit = await inputService.GetInputAsync() ?? "units";

            string date = await inputService.GetDateInputAsync();
            if (date == "0") return;

            int quantity = await inputService.GetNumberInputAsync($"\n\nPlease insert new amount (in {unit}):\n\n");

            HabitRecord updatedRecord = new HabitRecord
            {
                Id = recordId,
                HabitName = name,
                Unit = unit,
                Date = DateTime.ParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US")),
                Quantity = quantity
            };

            databaseManager.UpdateRecord(updatedRecord);
        }

        public async Task InsertAsync()
        {
            Console.WriteLine("\nEnter habit name (e.g. Running, Water):");
            string name = await inputService.GetInputAsync() ?? "Unknown";

            Console.WriteLine("Enter measurement unit (e.g. km, glasses):");
            string unit = await inputService.GetInputAsync() ?? "units";

            string date = await inputService.GetDateInputAsync();
            if (date == "0") return;

            int quantity = await inputService.GetNumberInputAsync($"\n\nPlease insert amount (in {unit}, no decimals allowed)\n\n");

            HabitRecord newRecord = new HabitRecord
            {
                HabitName = name,
                Unit = unit,
                Date = DateTime.ParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US")),
                Quantity = quantity
            };

            databaseManager.InsertRecord(newRecord);
        }
    }
}