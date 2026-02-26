using HabitTracker.CSamu7.Habits;
using HabitTracker.CSamu7.HabitsLog;

namespace HabitTracker.CSamu7.Validations
{
    public class IdLogHabitValidation
    {
        private HabitLogService _habitsLogService = new HabitLogService();
        public async Task<ValidationResult<int>> Validate(int idHabit)
        {
            HabitLogModel? habitModel = await _habitsLogService.GetLogHabit(idHabit);

            return habitModel is not null
                ? ValidationResult<int>.Success(idHabit)
                : ValidationResult<int>.Failure("Id doesn't exist");
        }
    }
}
