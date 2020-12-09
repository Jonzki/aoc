using Aoc.Problems.Aoc20;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc20
{
    [TestClass]
    public class Problem9Tests
    {
        [TestMethod]
        public void Part1_Small1_Works()
        {
            // suppose your preamble consists of the numbers 1 through 25 in a random order.
            var preamble = new long[25];
            for (var i = 0; i < preamble.Length; ++i) preamble[i] = i + 1;

            // Prepare an XMAS class.
            var x = new Problem9.XMAS(preamble);

            // 26 would be a valid next number, as it could be 1 plus 25 (or many other pairs, like 2 and 24).
            Assert.IsTrue(x.IsValid(26));

            // 49 would be a valid next number, as it is the sum of 24 and 25.
            Assert.IsTrue(x.IsValid(49));

            // 100 would not be valid; no two of the previous 25 numbers sum to 100.
            Assert.IsFalse(x.IsValid(100));

            // 50 would also not be valid; although 25 appears in the previous 25 numbers, the two numbers in the pair must be different.
            Assert.IsFalse(x.IsValid(50));
        }
    }
}