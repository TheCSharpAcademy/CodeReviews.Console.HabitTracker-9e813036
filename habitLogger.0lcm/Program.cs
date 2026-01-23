using Microsoft.Extensions.Logging;

namespace habitLogger._0lcm
{
    class Program
    {
        private static readonly ILogger Logger = AppLogger.CreateLogger<Program>();
        static void Main(string[] args)
        {
            SqliteHelper.CreateSqliteDatabase();

            const string menu = """
                Please enter An action from below.
                ---------------------------------
                new | -> Create a new habit type,
                view| -> View habits,
                exit| -> Exit this application,
                """;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("");
                MainScreenSelection selection = InputHelper.GetEnumInput(shortcutMap: EnumMaps.mainScreenSelectionMap, menu: menu);

                switch (selection)
                {
                    case MainScreenSelection.CreateNewHabit:
                        CreateNewHabit();
                        break;
                    case MainScreenSelection.ViewHabits:
                        ViewHabits();
                        break;
                    case MainScreenSelection.Exit:
                        System.Environment.Exit(0);
                        break;
                }
            }
        }

        static void ViewHabits()
        {
            Console.Clear();

            if (!TryShowHabits(out List<string> habitNames))
            {
                Console.WriteLine("There are currently no habits logged. Press enter to return to the main menu.");
                WaitForEnter();
                return;
            }

            string? selection = SelectHabitName(habitNames);
            if (selection is null) return;


            string habitName = selection;
            if (!TryLoadHabit(habitName, out var habit))
            {
                Logger.LogWarning("Habit '{habitName}' not found after selection.", habitName);
                Console.WriteLine("Habit could not be found.");
                WaitForEnter();
                return;
            }

            DisplayHabitInstances(habit!);

            ViewInstancesScreenSelection action = ShowInstanceActionMenu();

            List<int> instanceIds = SqliteHelper.GetInstanceIdsByParentId(habit!.Id);
            HandleInstanceAction(action, habit, instanceIds);
        }

        //------- CRUD - Habits -------
        static void CreateNewHabit()
        {
            string habitName = "";

            Console.Clear();

            bool isValidName = false;
            while (!isValidName)
            {
                Console.WriteLine("Please enter a name for your new habit, or 'return' to return:");
                habitName = InputHelper.GetStringInput();

                if (habitName == "return") return;

                isValidName = TryValidateHabitName(habitName);
            }

            Console.WriteLine("Please enter a unit type for your habit (e.g., times, minutes, reps):");
            string unitType = InputHelper.GetStringInput();

            ExecuteSafely(
                () => SqliteHelper.CreateNewHabit(new HabitCreate(habitName, unitType)),
                "Could not create a new habit"
                );

            Console.WriteLine("Habit created. Press enter to return to the main menu.");
            Console.ReadLine();
        }

        static void DeleteHabit(int id)
        {
            Console.WriteLine("Are you sure you want to delete the entire habit? (yes/no)");
            YesNoSelection confirmation = InputHelper.GetEnumInput(shortcutMap: EnumMaps.yesNoSelectionMap);

            if (confirmation == YesNoSelection.yes)
            {
                ExecuteSafely(
                    () => SqliteHelper.DeleteHabit(new HabitDelete(id)),
                    "Could not delete habit"
                    );
            }
            else
            {
                Console.WriteLine("Deletion cancelled");
                WaitForEnter();
            }
        }

        //------- CRUD - Habit Instances -------
        static void InsertNewHabitInstance(int parentId)
        {
            Console.Clear();

            Console.WriteLine("Please enter a quantity for your habit");
            int quantity = InputHelper.GetIntInput();

            Console.WriteLine("Please enter a date (d/m/y or m/d/y depending on your culture), or type today for todays date");
            DateTime date = InputHelper.GetDateTimeInput();

            ExecuteSafely(
                () => SqliteHelper.InsertNewHabitInstance(new HabitInstanceCreate(parentId, quantity, date)),
                "Could not insert new habit instance"
                );
        }

        static void UpdateHabitInstance(int parentId, List<int> instanceIds)
        {
            if (!SqliteHelper.HabitHasInstances(parentId))
            {
                Console.WriteLine("This action cannot be taken until you add some instances. Press enter to return to the main menu.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nPlease enter an instance ID to update");
            string instanceIdString = InputHelper.GetStringInput(validInputList: instanceIds.ConvertAll(id => id.ToString()));

            Console.Clear();

            Console.WriteLine("Please enter a new quantity for your habit");
            int quantity = InputHelper.GetIntInput();

            Console.WriteLine("Please enter a new date (d/m/y or m/d/y depending on your culture), or type today for todays date");
            DateTime date = InputHelper.GetDateTimeInput();
            string dateIso = date.ToString("yyyy-MM-dd");

            int.TryParse(instanceIdString, out int instanceId);
            ExecuteSafely(
                () => SqliteHelper.UpdateHabitInstance(new HabitInstance(parentId, instanceId, quantity, dateIso)),
                "Could not update habit instance"
                );
        }

        static void DeleteHabitInstance(int parentId, List<int> instanceIds)
        {
            if (!SqliteHelper.HabitHasInstances(parentId))
            {
                Console.WriteLine("This action cannot be taken until you add some instances. Press enter to return to the main menu.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nPlease enter an instance ID to delete");
            string instanceIdString = InputHelper.GetStringInput(validInputList: instanceIds.ConvertAll(id => id.ToString()));

            Console.WriteLine("Are you sure you want to delete this instance? This cannot be undone. (yes/no)");
            YesNoSelection confirmation = InputHelper.GetEnumInput(shortcutMap: EnumMaps.yesNoSelectionMap);

            if (confirmation == YesNoSelection.yes)
            {
                int.TryParse(instanceIdString, out int instanceId);
                ExecuteSafely(
                    () => SqliteHelper.DeleteHabitInstance(new HabitInstanceDelete(parentId, instanceId)),
                    "Could not delete instance"
                    );
            }
            else
            {
                Console.WriteLine("Deletion cancelled");
                WaitForEnter();
            }
        }

        static void DeleteAllHabitInstances(int parentId)
        {
            if (!SqliteHelper.HabitHasInstances(parentId))
            {
                Console.WriteLine("This action cannot be taken until you add some instances. Press enter to return to the main menu.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Are you sure you want to delete all instances of this habit? (yes/no)");
            YesNoSelection confirmation = InputHelper.GetEnumInput(shortcutMap: EnumMaps.yesNoSelectionMap);

            if (confirmation == YesNoSelection.yes)
            {
                ExecuteSafely(
                    () => SqliteHelper.DeleteAllHabitInstances(new HabitDelete(parentId)),
                    "Could not delete habit instances"
                    );
            }
            else
            {
                Console.WriteLine("Deletion cancelled");
                WaitForEnter();
            }
        }

        //------- Helper Methods -------
        private static bool TryShowHabits(out List<string> habitNames)
        {
            habitNames = SqliteHelper.HabitNamesToList();

            if (habitNames.Count == 0) return false;

            SqliteHelper.ShowHabits(); return true;
        }

        private static string? SelectHabitName(IReadOnlyList<string> habitNames)
        {
            var options = habitNames.ToList();
            options.Add("return");

            Console.WriteLine("Please write the name of a habit to see more options, or enter return to return to the main menu.");
            string selection = InputHelper.GetStringInput(validInputList: options, invalidInputPrompt: "enter a valid habit or 'return'");

            return selection.Equals("return", StringComparison.OrdinalIgnoreCase) ? null : selection;
        }

        private static bool TryLoadHabit(string habitName, out Habit? habit)
        {
            habit = SqliteHelper.GetHabitDataByName(habitName);
            return habit is not null;
        }

        private static void DisplayHabitInstances(Habit habit)
        {
            Console.Clear();

            var instanceIds = SqliteHelper.GetInstanceIdsByParentId(habit.Id);
            if (instanceIds.Count == 0)
            {
                Console.WriteLine("There are currently no instances of this habit logged.");
                return;
            }

            var instances = SqliteHelper.GetAllInstanceDataByInstanceIdAndParentId(habit.Id, instanceIds);

            Console.WriteLine("");
            foreach (var instance in instances!)
            {
                Console.WriteLine($"ID: {instance.InstanceId}. {habit.Name} - {instance.Quantity} {habit.UnitType} - {instance.DateIso}");
            }
            Console.WriteLine("\n");
        }

        private static ViewInstancesScreenSelection ShowInstanceActionMenu()
        {
            const string menu = """
                Please enter an action from below.
                ----------------------------------
                Insert      | -> insert a new habit instance,
                Update      | -> update a habit instance,
                Delete      | -> delete a habit instance,
                DeleteAll   | -> delete all instances of this habit,
                DeleteHabit | -> delete this habit and all its instances,
                Return      | -> return to the main menu,
                """;
            return InputHelper.GetEnumInput(shortcutMap: EnumMaps.viewInstancesScreenSelectionMap, menu: menu); 
        }

        private static void HandleInstanceAction(ViewInstancesScreenSelection action, Habit habit, List<int> instanceIds)
        {
            switch (action)
            {
                case ViewInstancesScreenSelection.InsertNewInstance:
                    InsertNewHabitInstance(habit.Id);
                    return;
                case ViewInstancesScreenSelection.UpdateInstance:
                    UpdateHabitInstance(habit.Id, instanceIds);
                    return;
                case ViewInstancesScreenSelection.DeleteInstance:
                    DeleteHabitInstance(habit.Id, instanceIds);
                    return;
                case ViewInstancesScreenSelection.DeleteAllInstances:
                    DeleteAllHabitInstances(habit.Id);
                    return;
                case ViewInstancesScreenSelection.DeleteHabit:
                    DeleteHabit(id: habit.Id);
                    return;
                case ViewInstancesScreenSelection.ReturnToMainMenu:
                    return;
                default:
                    Console.WriteLine("Error: Unknown input.. Returning to main menu.");
                    return;
            }
        }

        private static bool TryValidateHabitName(string habitName)
        {
            if (habitName == "return")
            {
                Console.WriteLine("Habit name cannot be 'Return' for technical reasons.");
                return false;
            }
            
            if (SqliteHelper.HabitNameExists(habitName))
            {
                Console.WriteLine("Habit name already exists.");
                return false;
            }
            return true;
        }

        private static void WaitForEnter(string message = "Press enter to continue.")
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        private static void ExecuteSafely(Action action, string errorMessage, string context = "")
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "{errorMessage} {context}", errorMessage, context);
                Console.WriteLine($"An error occured: {errorMessage}");
                WaitForEnter();
            }
        }
    }
}