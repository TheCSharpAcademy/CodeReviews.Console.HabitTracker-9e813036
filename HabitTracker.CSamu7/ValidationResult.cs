namespace HabitTracker.CSamu7
{
    public class ValidationResult<T>
    {
        private readonly T? _value;
        public List<string> Errors { get; init; }
        public bool IsValid { get; init; }
        public T Value
        {
            get
            {
                return IsValid ? _value! : throw new InvalidOperationException();
            }
        }
        private ValidationResult(T? result, List<string> errors, bool isValid)
        {
            Errors = errors;
            _value = result;
            IsValid = isValid;
        }
        public static ValidationResult<T> Success(T result)
        {
            return new ValidationResult<T>(result, [], true);
        }
        public static ValidationResult<T> Failure(List<string> errors)
        {
            return new ValidationResult<T>(default, errors, false);
        }
    }
}
