using System.Globalization;

namespace sql_management
{
    public static class DatabaseSeeder
    {
        public static void Seed(string connectionString)
        {
            var habits = new[]
            {
                new { Name = "Drink Water", Type = "fl oz" },
                new { Name = "Exercise", Type = "calories"},
                new { Name = "Read Book", Type = "count"}
            };

            var rand = new Random();

            foreach (var habit in habits)
            {
                try
                {
                    int index = habits.ToList().IndexOf(habit) + 1;

                    // Use your SqlCreate method to create the habit and its table
                    SqlCreate.InsertHabit(connectionString, habit.Name, habit.Type);

                    // Insert 10 records with random dates and quantities
                    for (int i = 0; i < 10; i++)
                    {
                        // Random date in the last 30 days
                        var date = DateTime.Now.AddDays(-rand.Next(0, 9125));
                        string dateStr = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                        // Random quantity between 10 and 100
                        int quantity = rand.Next(1, 10);

                        // Use your SqlCreate method to insert the record
                        SqlCreate.InsertRecord(connectionString, index, dateStr, quantity, habit.Type);
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