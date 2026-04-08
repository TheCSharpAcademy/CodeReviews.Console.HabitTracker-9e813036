using HabitTracker.Data;
using HabitTracker.Model;
using Microsoft.Data.Sqlite;
using Xunit;

namespace HabitTracker.Tests
{
    public class DatabaseManagerTests : IDisposable
    {
        private readonly string _testDbPath;
        private readonly string _connectionString;
        private readonly DatabaseManager _databaseManager;

        public DatabaseManagerTests()
        {
            _testDbPath = $"test_db_{Guid.NewGuid()}.db";
            _connectionString = $@"Data Source={_testDbPath}";
            _databaseManager = new DatabaseManager(_connectionString);

            _databaseManager.CreateTable();
        }

        [Fact]
        public void InsertRecord_And_GetAllRecords_ShouldAddAndRetrieveData()
        {
            var newRecord = new HabitRecord
            {
                HabitName = "Running",
                Unit = "km",
                Date = new DateTime(2024, 1, 1),
                Quantity = 5
            };

            _databaseManager.InsertRecord(newRecord);
            var records = _databaseManager.GetAllRecords();

            Assert.Single(records);
            Assert.Equal(newRecord.Quantity, records[0].Quantity);
            Assert.Equal(2024, records[0].Date.Year);
        }

        [Fact]
        public void DeleteRecord_ShouldRemoveRecordFromDatabase()
        {
            // Arrange
            _databaseManager.InsertRecord(new HabitRecord { HabitName = "Running", Unit = "km", Date = new DateTime(2024, 1, 1), Quantity = 5 });
            var records = _databaseManager.GetAllRecords();
            int idToDelete = records[0].Id;

            // Act
            bool isDeleted = _databaseManager.DeleteRecord(idToDelete);
            var recordsAfterDelete = _databaseManager.GetAllRecords();

            // Assert
            Assert.True(isDeleted);
            Assert.Empty(recordsAfterDelete);
        }

        [Fact]
        public void UpdateRecord_ShouldModifyExistingData()
        {
            // Arrange
            _databaseManager.InsertRecord(new HabitRecord { HabitName = "Running", Unit = "km", Date = new DateTime(2024, 1, 1), Quantity = 5 });
            var records = _databaseManager.GetAllRecords();
            int idToUpdate = records[0].Id;

            // Act
            _databaseManager.UpdateRecord(new HabitRecord { Id = idToUpdate, HabitName = "Reading", Unit = "pages", Date = new DateTime(2024, 1, 2), Quantity = 10 });
            var updatedRecords = _databaseManager.GetAllRecords();

            // Assert
            Assert.Equal(10, updatedRecords[0].Quantity);
            Assert.Equal(2, updatedRecords[0].Date.Day);
        }

        [Fact]
        public void CheckRecordExists_ShouldReturnTrueForExistingId()
        {
            // Arrange
            _databaseManager.InsertRecord(new HabitRecord { HabitName = "Running", Unit = "km", Date = new DateTime(2024, 1, 1), Quantity = 5 });
            var records = _databaseManager.GetAllRecords();
            int idToCheck = records[0].Id;

            // Act
            bool exists = _databaseManager.CheckRecordExists(idToCheck);

            // Assert
            Assert.True(exists);
        }

        public void Dispose()
        {
            SqliteConnection.ClearAllPools();

            if (File.Exists(_testDbPath))
            {
                File.Delete(_testDbPath);
            }
        }
    }
}