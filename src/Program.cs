namespace HabitTracker;

static class Program
{
    public static async Task Main(string[] args)
    {
        await StartProgram();
    }

    private static async Task StartProgram()
    {
        UserInterface.Intro();
        await UserInterface.AskPopulateTableWithRandomData();
        
        bool endApp = false;
        do
        {
            Console.WriteLine("----------");
            UserInterface.SpeechToggle();
            UserOption userOption = await UserInterface.AskUserOption();
            switch (userOption.Option)
            {
                case Option.Insert:
                    await UserInterface.AskInsert();
                    break;
                case Option.Delete:
                    await UserInterface.AskDelete();
                    break;
                case Option.Update:
                    await UserInterface.AskUpdate();
                    break;
                case Option.View:
                    await UserInterface.AskView();
                    break;
                case Option.SpeechToggle:
                    UserInterface.ToggleSpeech();
                    break;
                case Option.Quit:
                    endApp = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        } while (!endApp);

        UserInterface.GoodBye();
    }
}



