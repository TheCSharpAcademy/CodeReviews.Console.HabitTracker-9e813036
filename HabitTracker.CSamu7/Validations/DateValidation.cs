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
            List<string> errors = new List<string>();
            DateTimeOffset now = _timeProvider.GetLocalNow();
            DateTimeOffset dateParsed;

            if(!DateTimeOffset.TryParse(date, out dateParsed))
            {
                errors.Add("This is a invalid date. Example of valid date (yyyy/mm/dd)");
            }

            if (dateParsed > now)
            {
                errors.Add("The date must be in the present");
            }

            return errors.Count > 0
                ? ValidationResult<DateTimeOffset>.Failure(errors)
                : ValidationResult<DateTimeOffset>.Success(now);
        }
    }
}
