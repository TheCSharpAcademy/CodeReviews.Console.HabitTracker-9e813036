namespace habitLogger._0lcm;

public record Habit(
    int Id,
    string Name,
    string UnitType
    );

public record HabitCreate(
    string Name,
    string UnitType
    );

public record HabitDelete(
    int Id
    );

public record HabitInstance(
    int ParentId,
    int InstanceId,
    int Quantity,
    string DateIso
    );

public record HabitInstanceCreate(
    int ParentId,
    int Quantity,
    DateTime Date
    )
{
    public string DateIso => Date.ToString("yyyy-MM-dd");
}

public record HabitInstanceDelete(
        int ParentId,
        int InstanceId
        );
