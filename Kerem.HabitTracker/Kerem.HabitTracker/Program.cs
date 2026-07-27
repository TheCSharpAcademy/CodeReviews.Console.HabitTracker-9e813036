using Microsoft.Data.Sqlite;

namespace Kerem.HabitTracker
{
    class Program
    {
        static void Main()
        {
            
            DataAccess dataAccess = new DataAccess();
            SqliteConnection connection = dataAccess.EstablishConnection();
            ConsoleMenu startProgram = new ConsoleMenu();
            dataAccess.CreateHabitTable(connection);
            startProgram.Menu();
        }
    }
}