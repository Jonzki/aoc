using Aoc.Problems.Aoc19;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc19
{
    [TestClass]
    public class Problem4Tests
    {
        [DataTestMethod]
        [DataRow("111111", true)]
        [DataRow("223450", false)]
        [DataRow("123789", false)]
        public void Part1_SmallInput_Is_Correct(string password, bool valid)
        {
            Assert.AreEqual(valid, Problem4.IsValidPassword1(password));
        }

        [DataTestMethod]
        [DataRow("332211", false)]
        [DataRow("112233", true)]
        [DataRow("123444", false)]
        [DataRow("111122", true)]
        public void Part2_SmallInput_Is_Correct(string password, bool valid)
        {
            Assert.AreEqual(valid, Problem4.IsValidPassword2(password));
        }
    }
}