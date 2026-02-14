using HabitTracker.CSamu7.Validations;
using Microsoft.Extensions.Time.Testing;

namespace UnitTesting
{
    public class DateValidationTests
    {
        [Fact]
        public void Date_has_valid_format()
        {
            FakeTimeProvider timeProvider = new FakeTimeProvider(startDateTime: new DateTime(2026, 06, 06));
            DateValidation validation = new DateValidation(timeProvider);

            var result = validation.Validate("2025/10/24");

            Assert.True(result.IsValid);
            Assert.IsType<DateOnly>(result.Value);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Date_cant_be_in_the_future()
        {
            FakeTimeProvider timeProvider = new FakeTimeProvider(startDateTime: new DateTime(2026, 06, 06));
            DateValidation validation = new DateValidation(timeProvider);

            var result = validation.Validate("2026/06/07");

            Assert.False(result.IsValid);
            Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.Single(result.Errors);
        }
    }
}