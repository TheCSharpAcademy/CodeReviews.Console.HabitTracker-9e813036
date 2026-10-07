using System.Globalization;

namespace HabitTracker;

public static class UserInterface
{
    private static bool _isSpeechRecognitionActivated = false;
    
    public static void Intro()
    {
        Console.WriteLine("Welcome to my Habit Tracker app !");
    }
    public static void GoodBye()
    {
        Console.WriteLine("Good bye !");
    }

    public static void SpeechToggle()
    {
        Console.WriteLine($"Currently speech recognition is {(_isSpeechRecognitionActivated? "on" : "off")}");
    }
    
    public static async Task<UserOption> AskUserOption()
    {
        return await AskUser.Ask<UserOption>(
        ["Choose which operation you want to do:",
        "\tI - Insert a new habit",
        "\tD - Delete an existing habit",
        "\tU - Update an existing habit",
        "\tV - View your habits",
        "\tS - Toggle speech control",
        "\tQ - Leave the app"], 
            _isSpeechRecognitionActivated);
    }

    public static async Task AskInsert()
    {
        string name = await AskUser.Ask<string>(["Enter the name of the Habit you want to create"], _isSpeechRecognitionActivated);
        
        Habit? habit = DataBaseInterface.GetHabitByName(name);
        if (habit == null)
        {
            // The habit do not exist yet, ask for unit and add to DB
            string? unit = await AskUser.AskOptional([$"Enter the unit of the new Habit {name}, or nothing if you don't want an unit"], _isSpeechRecognitionActivated);
            habit = new Habit(name, unit);
            DataBaseInterface.InsertHabit(habit);
        }
        
        int quantity = await AskUser.Ask<int>([$"Enter the amount (in {habit.Unit ?? "no unit"})"], _isSpeechRecognitionActivated);
        string datePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower();
        DateTime? date = await AskUser.AskOptional<DateTime>([$"Enter the date ({datePattern}) this Habit is due, nothing for today's date"], _isSpeechRecognitionActivated);
        date ??= DateTime.Now;

        HabitRecord habitRecord = new HabitRecord(habit: habit, quantity: quantity, date: date);
        bool? confirm = await AskUser.AskOptional<bool>([$"You are trying to insert {habitRecord}","Enter False to dismiss it or nothing if you want to confirm"], _isSpeechRecognitionActivated);

        // We consider True and null as a confirmation
        if (confirm ?? true)
        {
            DataBaseInterface.InsertHabitRecord(habitRecord);
        }
    }

    public static async Task AskDelete()
    {
        int? index = await AskUser.AskOptional<int>(["Which habit to you want to delete ? Enter the index of the habit. Nothing to cancel"], _isSpeechRecognitionActivated);
        if (index == null) return;
        
        var records = DataBaseInterface.ViewHabits(id: index);
        if (records.Count == 0)
        {
            Console.WriteLine($"The {index} Habit doesn't exist");
            return;
        }

        HabitRecord habitRecord = records[0];
        
        bool? confirm = await AskUser.AskOptional<bool>([$"You are trying to delete {habitRecord}","Enter False to dismiss the deletion or nothing if you want to confirm"], _isSpeechRecognitionActivated);
        if ((confirm ?? true) && habitRecord.Id.HasValue)
        {
            DataBaseInterface.DeleteHabit(habitRecord.Id!.Value);
        }
    }

    public static async Task AskUpdate()
    {
        int? index = await AskUser.AskOptional<int>(["Which habit to you want to update ? Enter the index of the habit. Nothing to cancel"], _isSpeechRecognitionActivated);
        if (index == null) return;
        
        var records = DataBaseInterface.ViewHabits(id: index);
        if (records.Count == 0)
        {
            Console.WriteLine($"The {index} Habit doesn't exist");
            return;
        }

        HabitRecord oldHabitRecord = records[0];
        
        Console.WriteLine($"You are trying to update {oldHabitRecord}");
        
        int? newQuantity = await AskUser.AskOptional<int>([$"Enter the new amount of times this Habit should be done each week, or nothing to keep {oldHabitRecord.Quantity}"], _isSpeechRecognitionActivated);
        DateTime? newDate = await AskUser.AskOptional<DateTime>([$"Enter the new date (YYYY/mm/dd HH:mm:ss) this Habit happened, nothing to keep {oldHabitRecord.Date}"], _isSpeechRecognitionActivated);

        HabitRecord newHabitRecord = new HabitRecord(
            oldHabitRecord.Habit, 
            oldHabitRecord.Id,
            newQuantity ?? oldHabitRecord.Quantity,
            newDate ?? oldHabitRecord.Date);
        
        bool? confirm = await AskUser.AskOptional<bool>(
            [$"This is the updated Habit: {newHabitRecord}", 
                "Enter False to dismiss the update or nothing if you want to confirm"], _isSpeechRecognitionActivated);
        if ((confirm ?? true))
        {
            DataBaseInterface.UpdateHabit(newHabitRecord);
        }
        
    }

    public static async Task AskView()
    {
        Console.WriteLine("For the next fields, enter nothing to not filter by the corresponding parameter");
        int? index = await AskUser.AskOptional<int>(["Enter the index of the habit you want to see"], _isSpeechRecognitionActivated);
        string? name = await AskUser.AskOptional([$"Enter the name of the habit you want to see"], _isSpeechRecognitionActivated);
        string datePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower();
        DateTime? startDate = await AskUser.AskOptional<DateTime>([$"Enter the STARTING date ({datePattern}) of the habits"], _isSpeechRecognitionActivated);
        DateTime? endDate = await AskUser.AskOptional<DateTime>([$"Enter the ENDING date ({datePattern}) of the habits"], _isSpeechRecognitionActivated);
        
        Console.WriteLine("This is the list of Habits you wanted to see:");
        Console.WriteLine(string.Join("\n", DataBaseInterface.ViewHabits(index, name, startDate, endDate)));
    }

    public static async Task AskPopulateTableWithRandomData()
    {
        bool answer = await AskUser.AskOptional<bool>(["Do you want to populate the tables with random data ? nothing if not, true otherwise"],
            _isSpeechRecognitionActivated) ?? false;

        if (!answer) return;
        await DataBaseInterface.PopulateTableWithRandomData();
    }

    public static void ToggleSpeech()
    {
        if (ConfigSettings.IsConfigValid())
        {
            _isSpeechRecognitionActivated = !_isSpeechRecognitionActivated;
        }
        else
        {
            Console.WriteLine("The configuration is not right, can't activate speech. See README for more info");
        }
    }
}