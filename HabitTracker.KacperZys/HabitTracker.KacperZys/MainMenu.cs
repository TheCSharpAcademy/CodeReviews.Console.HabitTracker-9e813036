namespace HabitTracker.KacperZys;

internal class MainMenu
{
    DatabaseCommands databaseCommands = new DatabaseCommands();

    /// <summary>
    /// Displays the main menu and handles user input for various database operations.
    /// </summary>
    public void Display()
    {
        Console.WriteLine("You're working with a default database Named \"Hydration\"\n");

        bool continueRunning = true;

        while (continueRunning)
        {

            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("Type C to Close Application");
            Console.WriteLine("Type V to View All Records");
            Console.WriteLine("Type I to Insert Record");
            Console.WriteLine("Type D to Delete Record");
            Console.WriteLine("Type U to Update Record");
            Console.WriteLine("Type A to Add new habit");
            Console.WriteLine("Type H to Change habit");

            char userInput;
            try
            {
                userInput = Convert.ToChar(Console.ReadLine()!.ToUpper());
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a valid character.");
                continue;
            }

            switch (userInput)
            {
                case 'C':
                    Console.WriteLine("Closing Application.");
                    continueRunning = false;
                    break;
                case 'V':
                    databaseCommands.View();
                    break;
                case 'I':
                    databaseCommands.Insert();
                    break;
                case 'D':
                    databaseCommands.Delete();
                    break;
                case 'U':
                    databaseCommands.Update();
                    break;
                case 'A':
                    databaseCommands.AddHabit();
                    break;
                case 'H':
                    databaseCommands.ChooseHabit();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a valid option (C, V, I, D, U, A, H).");
                    break;
            }
        }
    }
}
