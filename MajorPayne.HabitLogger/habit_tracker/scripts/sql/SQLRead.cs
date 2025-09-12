using System;
using models;
using error_messages;


namespace sql_management
{
    public class SQLRead
    {
        private readonly string _connectionString;

        public SQLRead(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Record> ViewAllRecords(string tableName)
        {
            try
            {
                return SQLDatabaseHelper.ExecuteQuery(
                   _connectionString,
                   $"SELECT * FROM [{tableName}];",
                   reader => new Record
                   {
                       Id = reader.GetInt32(0),
                       Date = DateTime.Parse(reader.GetString(1)),
                       Quantity = reader.GetInt32(2),
                       Type = reader.IsDBNull(3) ? null : reader.GetString(3)
                   }
               );
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
                return SQLDatabaseHelper.ExecuteQuery(
                    _connectionString,
                    "SELECT * FROM habits;",
                    reader => new Habit
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Type = reader.GetString(2),
                        TableName = reader.GetString(3)
                    }
                );
            }
            catch (Exception ex)
            {
                DisplayError.ErrorMessage("Critical Error: " + ex.Message);
                throw;
            }

        }
    }
}