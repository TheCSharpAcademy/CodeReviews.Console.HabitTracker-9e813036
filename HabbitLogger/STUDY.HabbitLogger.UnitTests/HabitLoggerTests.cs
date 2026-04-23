using HabbitLogger;

namespace STUDY.HabbitLogger.UnitTests
{
    public class HabitLoggerTests
    {
        private static readonly object[] isValidHabitTextData =
        {
            new TestCaseData("Running", true),
            new TestCaseData("running", true),
            new TestCaseData(" Pages", true),
            new TestCaseData("Pages ", true),
            new TestCaseData(" Pages ", true),

            new TestCaseData(null, false),
            new TestCaseData("", false)
        };

        private static readonly object[] isValidHabitDate =
        {
            new TestCaseData("19-04-2025", true),
            new TestCaseData(" 19-04-2025", true),
            new TestCaseData("19-04-2025 ", true),
            new TestCaseData(" 19-04-2025 ", true),
            new TestCaseData("t", true),
            
            new TestCaseData("19/04/2025", false),
            new TestCaseData("19-04-25", false),
            new TestCaseData(null, false),
            new TestCaseData("", false)
        };

        private static readonly object[] isValidQuantity =
        {
            new TestCaseData("5", true),
            new TestCaseData(" 5", true),
            new TestCaseData("5 ", true),
            new TestCaseData(" 5 ", true),

            new TestCaseData("-1", false),
            new TestCaseData("abc", false),
            new TestCaseData(null, false),
            new TestCaseData("", false),
        };

        private static readonly object[] isValidNumberOfRows =
        {
            new TestCaseData(3, true),
            new TestCaseData(0, false)
        };

        private static readonly object[] isValidID =
        {
            new TestCaseData(2, true),
            new TestCaseData(-1, false)
        };

        [TestCaseSource(nameof(isValidHabitTextData))]
        public void CorrectHabitTextData_ReturnsExpectedResult(string? input, bool expectedValue)
        {
            bool isValidHabitTextData = ValidationHelper.ValidateHabitTextData(input);

            Assert.That(isValidHabitTextData, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(isValidHabitDate))]
        public void CorrectHabitDate_ReturnsExpectedResult(string? input, bool expectedValue)
        {
            bool isValidHabitDate = ValidationHelper.ValidateHabitDate(input);

            Assert.That(isValidHabitDate, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(isValidQuantity))]
        public void CorrectHabitQuantity_ReturnsExpectedResult(string? input, bool expectedValue)
        {
            bool isValidHabitQuantity = ValidationHelper.ValidateHabitQuantity(input);

            Assert.That(isValidHabitQuantity, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(isValidNumberOfRows))]
        public void CorrectNumberOfRows_ReturnsExpectedResult(int numberOfRow, bool expectedValue)
        {
            bool isValidNumberOfRows = ValidationHelper.ValidateNumberOfRows(numberOfRow);

            Assert.That(isValidNumberOfRows, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(isValidID))]
        public void CorrectID_ReturnsExpectedResult(int id, bool expectedValue)
        {
            bool isValidID = ValidationHelper.ValidateID(id);

            Assert.That(isValidID, Is.EqualTo(expectedValue));
        }
    }
}
