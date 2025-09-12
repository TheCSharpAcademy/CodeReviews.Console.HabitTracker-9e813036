using System;
using System.Globalization;
using sql_management;

namespace sql_management
{
    public static class DatabaseSeeder
    {
        public static void Seed(string connectionString)
        {
            var habits = new[]
            {
                new { Name = "Drink Water", Type = "fl oz", TableName = "drinking_water" },
                new { Name = "Exercise", Type = "calories", TableName = "exercise" },
                new { Name = "Read Book", Type = "count", TableName = "reading" }
            };

            var rand = new Random();

            foreach (var habit in habits)
            {
                try
                {
                    // Use your SQLCreate method to create the habit and its table
                    SQLCreate.InsertHabit(connectionString, habit.Name, habit.Type, habit.TableName);

                    // Insert 10 records with random dates and quantities
                    for (int i = 0; i < 200; i++)
                    {
                        // Random date in the last 30 days
                        var date = DateTime.Now.AddDays(-rand.Next(0, 9125));
                        string dateStr = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                        // Random quantity between 10 and 100
                        int quantity = rand.Next(10, 101);

                        // Use your SQLCreate method to insert the record
                        SQLCreate.InsertRecord(connectionString, habit.TableName, dateStr, quantity, habit.Type);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding habit '{habit.Name}': {ex.Message}");
                }
            }

            Console.WriteLine("Database seeded with sample habits and records.");
        }
    }
}