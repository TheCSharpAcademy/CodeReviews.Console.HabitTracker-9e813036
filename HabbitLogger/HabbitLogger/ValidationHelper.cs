using System.Globalization;

namespace HabbitLogger
{
    public static class ValidationHelper
    {
        public static bool ValidateHabitTextData(string? habitTextData)
        {
            bool success = true;

            if (habitTextData == null || habitTextData.Length == 0)
                success = false;

            return success;
        }

        public static bool ValidateHabitDate(string? userInput)
        {
            bool success = true;

            if (userInput == null || (userInput.Trim().ToLower() != "t" && !DateTime.TryParseExact(userInput.Trim().ToLower(), "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _)))
                success = false;

            return success;
        }

        public static bool ValidateHabitQuantity(string? userInput)
        {
            bool success = true;
            int quantity = -1;

            if (userInput == null || !int.TryParse(userInput, out quantity) || quantity < 0)
                success = false;

            return success;
        }

        public static bool ValidateNumberOfRows(int numberOfRows)
        {
            bool success = true;

            if (numberOfRows == 0)
            {
                success = false;
            }

            return success;
        }

        public static bool ValidateID(int idToVerify)
        {
            bool success = true;

            if (idToVerify == -1)
            {
                success = false;
            }

            return success;
        }
    }
}
