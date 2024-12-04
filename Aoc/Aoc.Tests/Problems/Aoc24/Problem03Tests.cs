using Aoc.Problems.Aoc24;

namespace Aoc.Tests.Problems.Aoc24
{
    [TestClass]
    public class Problem03Tests : ProblemTests<Problem03>
    {
        private const string SmallInput1 = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

        private const string SmallInput2 = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

        [TestMethod]
        public void SolvePart1()
        {
            RunPart1(161, SmallInput1);
        }

        [TestMethod]
        public void SolvePart2()
        {
            RunPart2(48, SmallInput2);
        }
    }
}