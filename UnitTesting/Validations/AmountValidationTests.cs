using HabitTracker.CSamu7.Validations;

namespace UnitTesting.Validations
{
    public class AmountValidationTests
    {
        [Fact]
        public void Amount_must_be_a_natural_number()
        {
            int AMOUNT = 4;
            AmountValidation sut = new AmountValidation();

            var result = sut.Validate(AMOUNT);

            Assert.True(result.IsValid);
            Assert.Equal(result.Value, AMOUNT);
            Assert.Empty(result.Error);
        }
        [Theory]
        [InlineData(-3)]
        [InlineData(0)]
        public void Amount_cant_be_negative(int amount)
        {
            AmountValidation sut = new AmountValidation();

            var result = sut.Validate(amount);

            Assert.False(result.IsValid);
            Assert.Single(result.Error);
        }
    }
}
