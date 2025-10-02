using models;
using error_messages;


namespace sql_management
{
    public class SqlRead
    {
        private readonly string _connectionString;

        public SqlRead(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Record> ViewAllRecords(int habitId)
        {
            try
            {
                var records = SqlDatabaseHelper.ExecuteQuery(
                    _connectionString,
                    "SELECT Id, HabitId, Date, Quantity, Type FROM records WHERE HabitId = @HabitId;",
                    reader =>
                    {
                        // Get column ordinals by name
                        int colRecordId = reader.GetOrdinal("Id");
                        int colHabitId = reader.GetOrdinal("HabitId");
                        int colDate = reader.GetOrdinal("Date");
                        int colQuantity = reader.GetOrdinal("Quantity");
                        int colType = reader.GetOrdinal("Type");

                        // Map reader to Record
                        return new Record
                        {
                            Id = reader.GetInt32(colRecordId),
                            HabitId = reader.GetInt32(colHabitId),
                            Date = DateTime.Parse(reader.GetString(colDate)),
                            Quantity = reader.GetInt32(colQuantity),
                            Type = reader.IsDBNull(colType) ? null : reader.GetString(colType)
                        };
                    },
                    cmd => cmd.Parameters.AddWithValue("@HabitId", habitId)
                );

                Console.WriteLine($"\nRecords for Habit ID {habitId}:");
                if (records.Count == 0)
                {
                    Console.WriteLine("No records found for this habit.");
                }
                else
                {
                    foreach (var record in records)
                    {
                        Console.WriteLine($"Record: {record.Id} | Date: {record.Date.ToShortDateString()} | Qty: {record.Quantity} {record.Type}");
                    }
                    
                }
                return records;
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("Critical Error: " + ex.Message);
                throw;
            }
        }

        public List<Habit> ViewAllHabits()
        {
            try
            {
                var habits = SqlDatabaseHelper.ExecuteQuery(
                    _connectionString,
                    "SELECT Id, Name, Type FROM habits;",
                    reader =>
                    {
                        int colId = reader.GetOrdinal("Id");
                        int colName = reader.GetOrdinal("Name");
                        int colType = reader.GetOrdinal("Type");

                        return new Habit
                        {
                            Id = reader.GetInt32(colId),
                            Name = reader.GetString(colName),
                            Type = reader.GetString(colType)
                        };
                    }
                );

                if (habits.Count == 0)
                {
                    Console.WriteLine("No habits found.");
                }
                else
                {
                    foreach (var habit in habits)
                    {
                        Console.WriteLine(
                            $"ID: {habit.Id} | Name: {habit.Name} | Type: {habit.Type}"
                        );
                    }
                }

                return habits;
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("Critical Error: " + ex.Message);
                return new List<Habit>();
            }


        }

    }
}
