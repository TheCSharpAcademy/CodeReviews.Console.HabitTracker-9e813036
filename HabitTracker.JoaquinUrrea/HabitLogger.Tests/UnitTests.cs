namespace HabitLogger.Tests
{
    public class UnitTests
    {
        [TestCase(1234)]
        [TestCase(2014)]
        [TestCase(4)]
        [TestCase(2024)]
        [TestCase(2012)]
        public void GivenYear_WhenLeapYear_ThenReturnsTrue(int year)
        {
            bool isLeapYear = Helpers.LeapYear(year);

            Assert.That(isLeapYear, Is.True);
        }

        [TestCase(2024, 12, 30, true)]
        [TestCase(2014, 2, 29, false)]
        [TestCase(4, 4, 4, true)]
        [TestCase(2026, 3, 17, false)]
        [TestCase(2012, 2, 29, true)]
        public void GivenDate_WhenDateIsValid_ThenReturnsFalse(int year, int month, int day, bool leapYear)
        {
            bool isInvalidDate = Helpers.InvalidDateCheck(year, month, day, leapYear);

            Assert.That(isInvalidDate, Is.False);
        }


    }
}
