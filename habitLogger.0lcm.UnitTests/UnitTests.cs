namespace habitLogger._0lcm.UnitTests
{
    public class InputHelperTests
    {
        public enum TestEnum
        {
            test, 
            test1,
            test2,
        }
        private static readonly Dictionary<string, TestEnum> testEnumMap = new(StringComparer.OrdinalIgnoreCase)
        {
            {"test", TestEnum.test},
            {"test1", TestEnum.test1},
            {"test2", TestEnum.test2},
        };

        [TestCase("string", false)]
        [TestCase(" string", false)]
        [TestCase("string ", false)]
        [TestCase(" string ", false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("123", true)]
        [TestCase(" 123", true)]
        [TestCase("123 ", true)]
        [TestCase(" 123 ", true)]
        public void GetIntInput_GivenInputIsInt_WhenMethodCalled_ThenReturnInt(string input, bool expectedResult)
        {
            var fakeInput = new StringReader(input);
            var fakeOutput = new StringWriter();

            Console.SetIn(fakeInput);
            Console.SetOut(fakeOutput);

            int? result;
            bool resultBool;

            try
            {
                result = InputHelper.GetIntInput(reader: fakeInput, maxAttempts: 5);
                resultBool = result is int;
            }
            catch (InvalidOperationException)
            {
                resultBool = false;
            }

            Assert.That(resultBool, Is.EqualTo(expectedResult));
        }

        //Tested under an american culture, D/M/Y expected false and M/D/Y expected true
        [TestCase("1/31/2025", true)]
        [TestCase(" 1/31/2025", true)]
        [TestCase("1/31/2025 ", true)]
        [TestCase(" 1/31/2025 ", true)]
        [TestCase("01/31/2025", true)]
        [TestCase("1/1/3000", true)]
        [TestCase("1/1/900000", false)]
        [TestCase("00/00/0000", false)]
        [TestCase("99/99/9999", false)]
        [TestCase("1111/1111/1111", false)]
        [TestCase("31/1/2025", false)]
        [TestCase(" 31/1/2025", false)]
        [TestCase("31/1/2025 ", false)]
        [TestCase(" 31/1/2025 ", false)]
        public void GetDateTimeInput_GivenValidInput_ThenReturnDateTime(string input, bool expectedResult)
        {
            var fakeInput = new StringReader(input);
            var fakeOutput = new StringWriter();

            Console.SetIn(fakeInput);
            Console.SetOut(fakeOutput);

            DateTime? result;
            bool resultBool;

            try
            {
                result = InputHelper.GetDateTimeInput(reader: fakeInput, maxAttempts: 5);
                resultBool = result is DateTime;
            }
            catch (InvalidOperationException)
            {
                resultBool = false; 
            }

            Assert.That(resultBool, Is.EqualTo(expectedResult));
        }

        [TestCase("test", TestEnum.test, true)]
        [TestCase("test1", TestEnum.test1, true)]
        [TestCase("test2", TestEnum.test2, true)]
        [TestCase(" test", TestEnum.test, true)]
        [TestCase("test ", TestEnum.test, true)]
        [TestCase(" test ", TestEnum.test, true)]
        [TestCase("TEST", TestEnum.test, true)]
        [TestCase("invalid", null, false)]
        [TestCase("", null, false)]
        [TestCase(" ", null, false)]
        public void GetEnumInput_GivenValidInput_ThenReturnEnumItem(string input, TestEnum? expectedEnumItem, bool expectedResult)
        {
            var fakeInput = new StringReader(input);
            var fakeOutput = new StringWriter();

            Console.SetIn(fakeInput);
            Console.SetOut(fakeOutput);

            TestEnum? result;
            bool resultBool;

            try
            {
                result = InputHelper.GetEnumInput(shortcutMap: testEnumMap, reader: fakeInput, maxAttempts: 5);
                resultBool = result is TestEnum;
            }
            catch (InvalidOperationException)
            {
                result = null;
                resultBool = false;
            }

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expectedEnumItem));
                Assert.That(resultBool, Is.EqualTo(expectedResult));
            });
        }
    }
}
