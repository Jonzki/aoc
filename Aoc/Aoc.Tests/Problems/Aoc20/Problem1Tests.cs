using Aoc.Problems.Aoc20;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc20
{
    [TestClass]
    public class Problem1Tests
    {
        [DataTestMethod]
        // First item is the target sum, rest are the number array.
        [DataRow(514579, 1721, 979, 366, 299, 675, 1456)]
        public void Part1_SmallInput_Is_Correct(object correctOutput, params object[] numbers)
        {
            var array = numbers.Select(x => (int)x).ToArray();
            Assert.AreEqual((int)correctOutput, Problem1.FindSumValue(array, 2020));
        }

        [DataTestMethod]
        // First item is the target sum, rest are the number array.
        [DataRow(241861950, 1721, 979, 366, 299, 675, 1456)]
        public void Part2_SmallInput_Is_Correct(object correctOutput, params object[] numbers)
        {
            var array = numbers.Select(x => (int)x).ToArray();
            Assert.AreEqual((int)correctOutput, Problem1.FindSumValue(array, 2020, 3));
        }
    }
}
