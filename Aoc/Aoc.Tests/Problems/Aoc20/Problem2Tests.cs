using Aoc.Problems.Aoc20;
using static Aoc.Problems.Aoc20.Problem2;

namespace Aoc.Tests.Problems.Aoc20
{
    [TestClass]
    public class Problem2Tests
    {
        [DataTestMethod]
        [DataRow("1-3 a: abcde", 1, 3, 'a', "abcde")]
        [DataRow("1-3 b: cdefg", 1, 3, 'b', "cdefg")]
        [DataRow("2-9 c: ccccccccc", 2, 9, 'c', "ccccccccc")]
        public void ParsePasswordRow_Parses_Row(object input, object correctMin, object correctMax, object correctChar, object correctPassword)
        {
            var parsed = Problem2.ParsePasswordRow((string)input);
            Assert.IsNotNull(parsed);
            Assert.AreEqual(correctMin, parsed.MinCharCount);
            Assert.AreEqual(correctMax, parsed.MaxCharCount);
            Assert.AreEqual(correctChar, parsed.RequiredChar);
            Assert.AreEqual(correctPassword, parsed.Password);
        }

        [DataTestMethod]
        [DataRow(true, 1, 3, 'a', "abcde")]
        [DataRow(false, 1, 3, 'b', "cdefg")]
        [DataRow(true, 2, 9, 'c', "ccccccccc")]
        public void IsPasswordValid1_Checks_Chars(object correctIsValid, object min, object max, object requiredChar, object password)
        {
            var passwordRow = new PasswordRow
            {
                MinCharCount = (int)min,
                MaxCharCount = (int)max,
                RequiredChar = (char)requiredChar,
                Password = (string)password
            };
            Assert.AreEqual((bool)correctIsValid, Problem2.IsPasswordValid1(passwordRow));
        }

        [DataTestMethod]
        [DataRow(true, 1, 3, 'a', "abcde")]
        [DataRow(false, 1, 3, 'b', "cdefg")]
        [DataRow(false, 2, 9, 'c', "ccccccccc")]
        public void IsPasswordValid2_Checks_Chars(object correctIsValid, object min, object max, object requiredChar, object password)
        {
            var passwordRow = new PasswordRow
            {
                MinCharCount = (int)min,
                MaxCharCount = (int)max,
                RequiredChar = (char)requiredChar,
                Password = (string)password
            };
            Assert.AreEqual((bool)correctIsValid, Problem2.IsPasswordValid2(passwordRow));
        }
    }
}