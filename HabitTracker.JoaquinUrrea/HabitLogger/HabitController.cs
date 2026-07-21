using System.Globalization;

namespace HabitLogger;

internal class HabitController
{
    private readonly HabitInterface habitInterface = new();

    internal void InsertHabitLog()
    {
        string habitSelected = SelectHabit(isInsert: true);

        if (habitSelected == "Insert new habit type")
        {
            var newHabitData = habitInterface.InsertNewHabitPrompt();

            string? name = newHabitData.Item1;
            string? quantityGoal = newHabitData.Item2;
            string? unit = newHabitData.Item3;

            name = name.ToLower();
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);

            if (!double.TryParse(quantityGoal, out double quantity) || name == "" || unit == "")
            {
                habitInterface.InvalidInputPrompt(input: "data");
            }
            else if (Helpers.CheckIfHabitExists(name: name))
            {
                habitInterface.HabitAlreadyExistsPrompt(input: name);
            }
            else
            {
                DatabaseFunctions.InsertHabitType(name: name, quantityGoal: quantity, unit: unit);
            }
        }
        else if (habitSelected == "---- RETURN TO MENU ----")
        {
            return;
        }
        else
        {
            InsertUpdateHabitLog(name: habitSelected, isInsert: true);
        }
    }

    internal void UpdateHabitLog()
    {
        InsertUpdateHabitLog(name: "", isInsert: false);
    }

    internal void DeleteHabitLog()
    {
        var logsRead = ReadHabitLogs(onlyRead: false);

        if (logsRead.Item2.Count == 0)
        {
            return;
        }
        else
        {
            var logSelected = logsRead.Item1;
            var logs = logsRead.Item2;

            DatabaseFunctions.DeleteHabitLog(id: logs[logSelected].Item1);

            habitInterface.SuccessPrompt(input: "deleted");
        }
    }

    internal (int, List<(int, string, double, bool, DateTime)>) ReadHabitLogs(bool onlyRead)
    {
        string habitSelected = SelectHabit(isInsert: false);

        if (habitSelected == "---- RETURN TO MENU ----")
        {
            return (0, []);
        }

        List<(int, string, double, bool, DateTime)> logs = DatabaseFunctions.ReadHabitLogs(habit: habitSelected);
        int logToUpdate = habitInterface.ShowHabitLogsTable(onlyRead: onlyRead, logs: logs);

        return (logToUpdate, logs);
    }

    internal string SelectHabit(bool isInsert)
    {
        List<string> habits = new();

        if (isInsert)
        {
            habits.Add("Insert new habit type");
        }

        habits.AddRange(DatabaseFunctions.ReadHabits(column: "name"));
        habits.Add("---- RETURN TO MENU ----");

        string habitSelected = habitInterface.SelectHabitPrompt(habits: habits);

        return habitSelected;
    }

    internal void InsertUpdateHabitLog(string name, bool isInsert)
    {
        var logsRead = (0, new List<(int, string, double, bool, DateTime)>());

        if (!isInsert)
        {
            logsRead = ReadHabitLogs(onlyRead: false);

            if (logsRead.Item2.Count == 0)
            {
                return;
            }
        }

        var habitLogData = habitInterface.InsertUpdateHabitLogPrompt();

        string? quantity = habitLogData.Item1;
        string? year = habitLogData.Item2;
        string? month = habitLogData.Item3;
        string? day = habitLogData.Item4;

        bool isQuantityParsed = double.TryParse(quantity, out double quantityParsed);
        bool isYearParsed = int.TryParse(year, out int yearParsed);
        bool isMonthParsed = int.TryParse(month, out int monthParsed);
        bool isDayParsed = int.TryParse(day, out int dayParsed);

        if (!isQuantityParsed || !isYearParsed || !isMonthParsed || !isDayParsed)
        {
            habitInterface.InvalidInputPrompt(input: "data");
            return;
        }

        if (Helpers.InvalidDateCheck(year: yearParsed, month: monthParsed, day: dayParsed, Helpers.LeapYear(year: yearParsed)))
        {
            habitInterface.InvalidInputPrompt(input: "date");
        }
        else
        {
            if (isInsert)
            {
                DatabaseFunctions.InsertHabitLog(name: name, quantity: quantityParsed, goalAchieved: Helpers.GoalAchieved(quantityGoal: Helpers.GetQuantityGoal(habitName: name), quantity: quantityParsed),
                    date: Helpers.FormatDate(yearParsed, monthParsed, dayParsed));

                habitInterface.SuccessPrompt(input: "inserted");
            }
            else
            {
                var logSelected = logsRead.Item1;
                var logs = logsRead.Item2;

                DatabaseFunctions.UpdateHabitLog(id: logs[logSelected].Item1, quantity: quantityParsed, goalAchieved: Helpers.GoalAchieved(quantityGoal: Helpers.GetQuantityGoal(habitName: logs[logSelected].Item2),
                    quantity: quantityParsed), date: Helpers.FormatDate(yearParsed, monthParsed, dayParsed));

                habitInterface.SuccessPrompt(input: "updated");
            }
        }
    }
}
