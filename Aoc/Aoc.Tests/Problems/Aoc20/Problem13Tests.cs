using Aoc.Problems.Aoc20;

namespace Aoc.Tests.Problems.Aoc20
{
    [TestClass]
    public class Problem13Tests
    {
        [TestMethod]
        public void Part1_SmallInput_Works()
        {
            // Test individual starting points.
            var timestamp = 939;

            var next = Problem13.GetNextDeparture(7, timestamp, 0);
            Assert.AreEqual(945, next);

            next = Problem13.GetNextDeparture(13, timestamp, 0);
            Assert.AreEqual(949, next);

            next = Problem13.GetNextDeparture(59, timestamp, 0);
            Assert.AreEqual(944, next);

            var input = "939\n7,13,x,x,59,x,31,19";
            var result = new Problem13().Solve1(input);
            Assert.AreEqual(295, result);
        }

        [DataTestMethod]
        [DataRow("7,13,x,x,59,x,31,19", 1068781)]
        [DataRow("17,x,13,19", 3417)]
        [DataRow("67,7,59,61", 754018)]
        [DataRow("67,x,7,59,61", 779210)]
        [DataRow("67,7,x,59,61", 1261476)]
        [DataRow("1789,37,47,1889", 1202161486)]
        public void Part2_SmallInput_Works(string input, long correctTimestamp)
        {
            Assert.Inconclusive("Problem is not implemented.");

            //var result = new Problem13().Solve2("-\n" + input);
            //Assert.AreEqual(correctTimestamp, result);
        }
    }
}