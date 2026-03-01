namespace HabitTracker.CSamu7.Validations
{
    public class AmountValidation
    {
        public ValidationResult<int> Validate(int amount)
        {
            if (amount < 0)
                return ValidationResult<int>.Failure("The amount must be a positive amount");

            if (amount == 0)
                return ValidationResult<int>.Failure("The amount can't be zero");

            return ValidationResult<int>.Success(amount);
        }
    }
}
