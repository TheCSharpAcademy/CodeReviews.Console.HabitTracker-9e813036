using HabitLogger;

HabitInterface habitInterface = new();

Helpers.GenerateDatabase();

do
{
    bool exit = habitInterface.MainMenu();
    if (exit)
    {
        break;
    }
} while (true);
