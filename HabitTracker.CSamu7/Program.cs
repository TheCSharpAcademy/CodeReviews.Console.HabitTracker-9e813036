using HabitTracker.CSamu7;

MainMenu mainMenu = new MainMenu();

while (true)
{
    Console.WriteLine("===HABIT TRACKER===");
    HabitDb db = new HabitDb();
    db.CreateSchema();
    mainMenu.DisplayMenu();
}