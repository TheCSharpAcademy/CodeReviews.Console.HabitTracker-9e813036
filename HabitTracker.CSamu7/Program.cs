using HabitTracker.CSamu7;
using HabitTracker.CSamu7.Database;

MainMenu mainMenu = new MainMenu();
DbCreation dbCreation = new DbCreation();

await dbCreation.Create();

while (true)
{
    mainMenu.DisplayMenu();
}