using Aoc.Problems.Aoc25;
using static Aoc.Problems.Aoc18.Problem10;

namespace Aoc.Tests.Problems.Aoc25
{
    [TestClass]
    public class Problem03Tests : ProblemTests<Problem03>
    {
        private const string SmallInput = """
                                          987654321111111
                                          811111111111119
                                          234234234234278
                                          818181911112111
                                          """;

        [TestMethod]
        public void SolvePart1()
        {
            RunPart1(357, SmallInput);
        }

        [TestMethod]
        public void SolvePart2()
        {
            RunPart2(3121910778619L, SmallInput);
        }

        [TestMethod]
        public void FindLargestNumber_WorksFor_Basic()
        {
            // Single digit should work fine.
            Problem03.FindLargestNumber("1", 1).Should().Be(1L);

            // Two digits is also trivial.
            Problem03.FindLargestNumber("15", 2).Should().Be(15L);

            // Three digits is the first where we can have some variance.
            Problem03.FindLargestNumber("110", 2).Should().Be(11L);
            Problem03.FindLargestNumber("101", 2).Should().Be(11L);

            Problem03.FindLargestNumber("102", 2).Should().Be(12L);
            Problem03.FindLargestNumber("120", 2).Should().Be(20L);

            // Should work for longer numbers too.
            Problem03.FindLargestNumber("10001000", 4).Should().Be(1100L);
            Problem03.FindLargestNumber("10000001", 4).Should().Be(1001L);

            Problem03.FindLargestNumber("78907901", 4).Should().Be(9901L);
        }

        [DataTestMethod]
        [DataRow("987654321111111", 98)]
        [DataRow("811111111111119", 89)]
        [DataRow("234234234234278", 78)]
        [DataRow("818181911112111", 92)]
        public void FindLargestNumber_WorksFor_TaskInputs_Part1(string input, int correctOutput)
        {
            var output = Problem03.FindLargestNumber(input, 2);
            output.Should().Be(correctOutput);
        }

        [DataTestMethod]
        [DataRow("987654321111111", 987654321111L)]
        [DataRow("811111111111119", 811111111119L)]
        [DataRow("234234234234278", 434234234278L)]
        [DataRow("818181911112111", 888911112111L)]
        public void FindLargestNumber_WorksFor_TaskInputs_Part2(string input, long correctOutput)
        {
            var output = Problem03.FindLargestNumber(input, 12);
            output.Should().Be(correctOutput);
        }
    }
}