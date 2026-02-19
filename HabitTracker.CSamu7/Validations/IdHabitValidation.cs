using HabitTracker.CSamu7.Habits;

namespace HabitTracker.CSamu7.Validations
{
    public class IdHabitValidation
    {
        private HabitService _habitsService = new HabitService();
        public async Task<ValidationResult<int>> Validate(int idHabit)
        {
            HabitModel? habitModel = await _habitsService.GetHabit(idHabit);

            return habitModel is not null
                ? ValidationResult<int>.Success(idHabit)
                : ValidationResult<int>.Failure(["Id doesn't exist"]);
        }
    }
}
