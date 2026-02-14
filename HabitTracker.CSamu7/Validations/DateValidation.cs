namespace HabitTracker.CSamu7.Validations
{
    public class DateValidation
    {
        private TimeProvider _timeProvider;
        public DateValidation(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public ValidationResult<DateOnly> Validate(string date)
        {
            List<string> errors = new List<string>();
            DateTimeOffset now = _timeProvider.GetLocalNow();
            DateOnly dateNow = new(now.Year, now.Month, now.Day);
            DateOnly dateParsed;

            if(!DateOnly.TryParse(date, out dateParsed))
            {
                errors.Add("This is a invalid date. Valid date (yyyy/mm/dd)");
            }

            if (dateParsed > dateNow)
            {
                errors.Add("The date must be in the present");
            }

            return errors.Count > 0
                ? ValidationResult<DateOnly>.Failure(errors)
                : ValidationResult<DateOnly>.Success(dateParsed);
        }
    }
}
