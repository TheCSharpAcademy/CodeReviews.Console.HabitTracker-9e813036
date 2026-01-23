namespace habitLogger._0lcm
{
    public enum MainScreenSelection
    {
        CreateNewHabit,
        ViewHabits,
        Exit,
    }
    public enum ViewInstancesScreenSelection
    {
        DeleteInstance,
        InsertNewInstance,
        UpdateInstance,
        DeleteAllInstances,
        DeleteHabit,
        ReturnToMainMenu,
    }
    public enum YesNoSelection
    {
        yes,
        no,
    }
    public class EnumMaps
    {
        public static readonly Dictionary<string, MainScreenSelection> mainScreenSelectionMap = new(StringComparer.OrdinalIgnoreCase)
        {
            {"new", MainScreenSelection.CreateNewHabit},
            {"view", MainScreenSelection.ViewHabits},
            {"exit", MainScreenSelection.Exit},
        };
        public static readonly Dictionary<string, ViewInstancesScreenSelection> viewInstancesScreenSelectionMap = new(StringComparer.OrdinalIgnoreCase)
        {
            {"delete", ViewInstancesScreenSelection.DeleteInstance},
            {"del", ViewInstancesScreenSelection.DeleteInstance},
            {"insert", ViewInstancesScreenSelection.InsertNewInstance},
            {"update", ViewInstancesScreenSelection.UpdateInstance},
            {"deleteAll", ViewInstancesScreenSelection.DeleteAllInstances},
            {"del all", ViewInstancesScreenSelection.DeleteAllInstances},
            {"delAll", ViewInstancesScreenSelection.DeleteAllInstances },
            {"delete all", ViewInstancesScreenSelection.DeleteAllInstances},
            {"deleteHabit", ViewInstancesScreenSelection.DeleteHabit},
            {"del habit", ViewInstancesScreenSelection.DeleteHabit},
            {"delHabit", ViewInstancesScreenSelection.DeleteHabit},
            {"delete habit", ViewInstancesScreenSelection.DeleteHabit},
            {"return", ViewInstancesScreenSelection.ReturnToMainMenu},
        };
        public static readonly Dictionary<string, YesNoSelection> yesNoSelectionMap = new(StringComparer.OrdinalIgnoreCase)
        {
            {"yes", YesNoSelection.yes},
            {"y", YesNoSelection.yes},
            {"no", YesNoSelection.no},
            {"n", YesNoSelection.no},
        };
    }
}
