using Aoc.Problems.Aoc25;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc25;

[TestClass]
public class Problem04Tests : ProblemTests<Problem04>
{
    private const string SmallInput = """
                                      ..@@.@@@@.
                                      @@@.@.@.@@
                                      @@@@@.@.@@
                                      @.@@@@..@.
                                      @@.@@@@.@@
                                      .@@@@@@@.@
                                      .@.@.@.@@@
                                      @.@@@.@@@@
                                      .@@@@@@@@.
                                      @.@.@@@.@.
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(13, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(43, SmallInput);
    }

    [TestMethod]
    public void ParseMap_Works()
    {
        var input = """
                    ...
                    @..
                    ..@
                    """;

        var map = Problem04.ParseMap(input);

        map.Width.Should().Be(3);
        map.Height.Should().Be(3);

        map.Data[0, 1].Should().Be('@');
        map.Data[2, 2].Should().Be('@');
    }
}