namespace HabitTracker.CSamu7.Validations
{
    public class DateValidation
    {
        private TimeProvider _timeProvider;
        public DateValidation(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public ValidationResult<DateTimeOffset> Validate(string date)
        {
            DateTimeOffset now = _timeProvider.GetLocalNow();
            DateTimeOffset dateParsed;

            if (!DateTimeOffset.TryParse(date, out dateParsed))
                return ValidationResult<DateTimeOffset>.Failure("This is a invalid date. Example of valid date (yyyy/mm/dd)");

            if (dateParsed > now)
                return ValidationResult<DateTimeOffset>.Failure("The date must be in the present");

            return ValidationResult<DateTimeOffset>.Success(now);
        }
    }
}
