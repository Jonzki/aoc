using Aoc.Problems.Aoc24;

namespace Aoc.Tests.Problems.Aoc24
{
    [TestClass]
    public class Problem09Tests : ProblemTests<Problem09>
    {
        public const string SmallInput = "2333133121414131402";

        public const string MiniInput = "12345";

        [TestMethod]
        public void SolvePart1()
        {
            RunPart1(1928L, SmallInput);
        }

        [TestMethod]
        public void SolvePart2()
        {
            RunPart2(2858L, SmallInput);
        }

        [TestMethod]
        public void ParseInput_ProducesCorrectDisk()
        {
            var disk = Problem09.ParseInput(MiniInput);

            var diskString = Problem09.DiskToString(disk);

            diskString.Should().Be("0..111....22222");
        }
    }
}