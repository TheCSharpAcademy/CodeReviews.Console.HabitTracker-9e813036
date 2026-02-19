namespace HabitTracker.CSamu7.Validations
{
    public class AmountValidation
    {
        public ValidationResult<int> Validate(int amount)
        {
            List<string> errors = new List<string>();

            if (amount < 0)
            {
                errors.Add("The amount must be a positive amount");
            }else if(amount == 0)
            {
                errors.Add("The amount can't be zero");
            }

            return errors.Count == 0
                ? ValidationResult<int>.Success(amount)
                : ValidationResult<int>.Failure(errors);
        }
    }
}
