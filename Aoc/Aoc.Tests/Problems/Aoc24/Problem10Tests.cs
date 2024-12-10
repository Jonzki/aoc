using Aoc.Problems.Aoc24;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc24
{
    [TestClass]
    public class Problem10Tests : ProblemTests<Problem10>
    {
        public const string MiniInput = """
                                        0123
                                        1234
                                        8765
                                        9876
                                        """;

        public const string SmallInput = """
                                         89010123
                                         78121874
                                         87430965
                                         96549874
                                         45678903
                                         32019012
                                         01329801
                                         10456732
                                         """;

        [TestMethod]
        public void SolvePart1()
        {
            RunPart1(36, SmallInput);
        }

        [TestMethod]
        public void SolvePart2()
        {
            RunPart2(81, SmallInput);
        }

        [TestMethod]
        public void CanReach_WorksFor_MiniInput()
        {
            var map = Problem10.ParseMap(MiniInput);

            var start = new Point2D(0, 0);

            // 0,3 has a 9 value, should be reachable.
            var end = new Point2D(0, 3);
            Problem10.CanReach(map, start, end).Should().BeTrue();
        }

        [TestMethod]
        public void CanReach_WorksFor_SmallInput()
        {
            var map = Problem10.ParseMap(SmallInput);

            Problem10.CanReach(map, new Point2D(2, 0), new Point2D(1, 0)).Should().BeTrue();
            Problem10.CanReach(map, new Point2D(2, 0), new Point2D(4, 5)).Should().BeTrue();
            Problem10.CanReach(map, new Point2D(2, 0), new Point2D(4, 6)).Should().BeFalse();
        }

        [TestMethod]
        public void FindPaths_WorksFor_SmallInput()
        {
            const string input = """
                                 .....0.
                                 ..4321.
                                 ..5..2.
                                 ..6543.
                                 ..7..4.
                                 ..8765.
                                 ..9....
                                 """;

            var map = Problem10.ParseMap(input);

            var start = new Point2D(5, 0);
            var end = new Point2D(2, 6);

            var paths = Problem10.FindPaths(map, end, new Problem10.Path(start));

            paths.Should().HaveCount(3);
        }
    }
}