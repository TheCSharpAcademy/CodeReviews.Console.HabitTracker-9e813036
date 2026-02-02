using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace habitLogger._0lcm
{
    internal class SqliteHelper
    {
        private const string databaseFileName = "habits.db";
        private const string connectionString = $"Data Source={databaseFileName}";

        private static readonly ILogger Logger = AppLogger.CreateLogger<SqliteHelper>();

        //------- Connection Factory -------
        private static SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }

        //------- Execute Helper Methods -------
        private static void ExecuteNonQuery(string sqlCommand, params (string name, object? value)[] parameters)
        {
            using var connection = CreateOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = sqlCommand;

            foreach (var (name, value) in parameters)
            {
                command.Parameters.AddWithValue(name, value);
            }

            command.ExecuteNonQuery();
        }

        private static T? ExecuteScalar<T>(string sqlCommand, params (string name, object? value)[] parameters)
        {
            using var connection = CreateOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = sqlCommand;

            foreach (var (name, value) in parameters)
            {
                command.Parameters.AddWithValue(name, value);
            }

            var result = command.ExecuteScalar();
            return result is T t ? t : default;
        }

        //------- Initilize Database -------
        public static void CreateSqliteDatabase()
        {
            const string createTableCommand = @"
                CREATE TABLE IF NOT EXISTS habits (
                    id          INTEGER NOT NULL UNIQUE,
                    name        TEXT NOT NULL UNIQUE,
                    unitType    TEXT NOT NULL,
                    PRIMARY KEY(id AUTOINCREMENT)
                );";

            const string createInstanceTableCommand = @"
                CREATE TABLE IF NOT EXISTS habitInstances (
                    parentId    INTEGER NOT NULL,
                    instanceId  INTEGER NOT NULL,
                    quantity    INTEGER NOT NULL,
                    date        TEXT NOT NULL,
                    PRIMARY KEY(instanceId AUTOINCREMENT)
                );";

            try
            {
                ExecuteNonQuery(createTableCommand);
                ExecuteNonQuery(createInstanceTableCommand);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not create database tables");
                throw;
            }
        }

        //------- CRUD - Habits -------
        public static void CreateNewHabit(HabitCreate habit)
        {
            const string insertCommand = @"
                        INSERT INTO habits (name, unitType)
                        VALUES (@name, @unitType);
                        ";
            try
            {
                ExecuteNonQuery(insertCommand, ("@name", habit.Name), ("@unitType", habit.UnitType));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not create new habit");
                throw;
            }
        }

        public static void DeleteHabit(HabitDelete habit)
        {
            const string deleteCommand = "DELETE FROM habits WHERE id = @id";

            try
            {
               ExecuteNonQuery(deleteCommand, ("@id", habit.Id));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not delete habit");
                throw;
            }
        }

        //------- CRUD - Habit Instances -------
        public static void InsertNewHabitInstance(HabitInstanceCreate habitInstance)
        {
            const string insertCommand = "INSERT INTO habitInstances (parentId, quantity, date) VALUES (@parentId, @quantity, @date)";
            try
            {
                ExecuteNonQuery(insertCommand, 
                    ("@parentId", habitInstance.ParentId), 
                    ("@quantity", habitInstance.Quantity), 
                    ("@date", habitInstance.DateIso));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not insert new habit instance");
                throw;
            }
        }

        public static void UpdateHabitInstance(HabitInstance habitInstance)
        {
            const string updateCommand = @"
                        UPDATE habitInstances
                        SET quantity = @quantity, date = @date
                        WHERE parentId = @parentId 
                        AND instanceId = @instanceId
                        ";

            try
            {
                ExecuteNonQuery(updateCommand, 
                    ("@quantity", habitInstance.Quantity), 
                    ("@date", habitInstance.DateIso), 
                    ("@parentId", habitInstance.ParentId), 
                    ("@instanceId", habitInstance.InstanceId));
            }
            catch (Exception ex) 
            { 
                Logger.LogError(ex, "Could not update habit instance");
                throw;
            }
        }

        public static void DeleteHabitInstance(HabitInstanceDelete habitInstance)
        {
            const string deleteCommand = @"
                        DELETE FROM habitInstances
                        WHERE parentId = @parentId
                        AND instanceId = @instanceId
                    ";

            try
            {
                ExecuteNonQuery(deleteCommand, 
                    ("@parentId", habitInstance.ParentId), 
                    ("@instanceId", habitInstance.InstanceId));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not delete habit instance");
                throw;
            }
        }

        public static void DeleteAllHabitInstances(HabitDelete habit)
        {
            const string deleteCommand = @"
                        DELETE FROM habitInstances
                        WHERE parentId = @parentId
                        ";

            try
            {
                ExecuteNonQuery(deleteCommand, ("@parentId", habit.Id));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not delete all habit instances for parentId {ParentId}", habit.Id);
                throw;
            }
        }

        //------- Queries and Read Operations -------
        public static bool IsHabitsTableEmpty()
        {
            const string isEmptyQuery = "SELECT EXISTS (SELECT 1 FROM habits)";
            try
            {
                var result = ExecuteScalar<long>(isEmptyQuery);
                return result is not 1L;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not determine if habits table is empty");
                throw;
            }
        }

        public static bool HabitHasInstances(int parentId)
        {
            const string hasInstancesQuery = "SELECT EXISTS (SELECT 1 FROM habitInstances WHERE parentId = @parentId)";

            try
            {
                var result = ExecuteScalar<long>(hasInstancesQuery, (@"parentId", parentId));
                return result is 1L;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not determine if habit has instances for parentId {ParentId}", parentId);
                throw;
            }
        }

        public static bool HabitNameExists(string habitName)
        {
            const string nameExistsQuery = "SELECT EXISTS (SELECT 1 FROM habits WHERE LOWER(name) = @name)";

            try
            {
                var result = ExecuteScalar<long>(nameExistsQuery, ("@name", habitName.ToLower()));
                return result is 1L;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not determine if habit name exists: {HabitName}", habitName);
                throw;
            }
        }

        public static List<string> HabitNamesToList()
        {
            const string readCommand = "SELECT name FROM habits";
            List<string> names = new();

            try
            {
                using var connection = CreateOpenConnection();
                using var command = connection.CreateCommand();
                command.CommandText = readCommand;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    names.Add(reader.GetString(0));
                }
                return names;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not parse habit names to list: {ex.Message}");
                throw;
            }
        }

        public static void ShowHabits()
        {
            const string showCommand = "SELECT name FROM habits";

            try
            {
                using var connection = CreateOpenConnection();
                using var command = connection.CreateCommand();
                command.CommandText = showCommand;

                using var reader = command.ExecuteReader();
                if (!reader.HasRows) return;

                int counter = 1;
                while (reader.Read())
                {
                    Console.WriteLine($"{counter}. {reader.GetString(0)}");
                    counter++;
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not show habits");
                throw;
            }
        }

        /// <summary>
        /// Returns [id, name, unitType] or empty list if cannot be found
        /// </summary>
        public static Habit? GetHabitDataByName(string habitName)
        {
            const string getDataCommand = "SELECT id, name, unitType FROM habits WHERE LOWER(name) = @name";

            try
            {
                using var connection = CreateOpenConnection();
                using var command = connection.CreateCommand();
                command.CommandText = getDataCommand;
                command.Parameters.AddWithValue("@name", habitName);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                return new Habit(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2)
                    );
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not get habit data by name {HabitName}", habitName);
                throw;
            }
        }

        public static List<int> GetInstanceIdsByParentId(int parentId)
        {
            const string getIdsCommand = "SELECT instanceId FROM habitInstances WHERE parentId = @parentId";
            List<int> ids = new();

            try
            {
                using var connection = CreateOpenConnection();
                using var command = connection.CreateCommand();
                command.CommandText = getIdsCommand;
                command.Parameters.AddWithValue("@parentId", parentId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ids.Add(reader.GetInt32(0));
                }
                return ids;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not get instance ids: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Returns a list of HabitInstance types or null if no instance found
        /// </summary>
        public static List<HabitInstance>? GetAllInstanceDataByInstanceIdAndParentId(int parentId, IReadOnlyList<int> instanceIds)
        {
            const string getDataCommand = @"
                        SELECT quantity, date
                        FROM habitInstances
                        WHERE parentId = @parentId
                        AND instanceId = @instanceId
                        ";
            List<HabitInstance> instances = new();

            try
            {
                if (instanceIds.Count == 0) return [];

                using var connection = CreateOpenConnection();
                using var command = connection.CreateCommand();
                command.CommandText = getDataCommand;

                var parentIdParam = command.Parameters.AddWithValue("@parentId", System.Data.DbType.Int32);
                var instanceIdParam = command.Parameters.AddWithValue("@instanceId", System.Data.DbType.Int32);
                foreach(var id in instanceIds)
                {
                    parentIdParam.Value = parentId;
                    instanceIdParam.Value = id;

                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    instances.Add(new HabitInstance(
                            ParentId:       parentId,
                            InstanceId:     id,
                            Quantity:       reader.GetInt32(0),
                            DateIso:        reader.GetString(1)
                        ));
                }
                return instances;
                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not get all instance data for parentId {ParentId}", parentId);
                throw;
            }
        }
    }
}