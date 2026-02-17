namespace HabitTracker.CSamu7.Validations
{
    public class StringValidation
    {
        public bool Validate(string str)
        {
            if(string.IsNullOrEmpty(str)) return false;

            return true;
        }
    }
}
