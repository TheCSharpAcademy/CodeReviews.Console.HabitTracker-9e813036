namespace HabitTracker.CSamu7.Validations
{
    public class ValidationResult<T>
    {
        private readonly T? _value;
        public string Error { get; init; }
        public bool IsValid { get; init; }
        public T Value
        {
            get
            {
                return IsValid ? _value! : throw new InvalidOperationException();
            }
        }
        private ValidationResult(T? result, string error, bool isValid)
        {
            Error = error;
            _value = result;
            IsValid = isValid;
        }
        public static ValidationResult<T> Success(T result)
        {
            return new ValidationResult<T>(result, String.Empty, true);
        }
        public static ValidationResult<T> Failure(string error)
        {
            return new ValidationResult<T>(default, error, false);
        }
    }
}
