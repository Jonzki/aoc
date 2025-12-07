using Aoc.Problems.Aoc25;

namespace Aoc.Tests.Problems.Aoc25;

[TestClass]
public class Problem07Tests : ProblemTests<Problem07>
{
    private const string SmallInput = """
                                      .......S.......
                                      ...............
                                      .......^.......
                                      ...............
                                      ......^.^......
                                      ...............
                                      .....^.^.^.....
                                      ...............
                                      ....^.^...^....
                                      ...............
                                      ...^.^...^.^...
                                      ...............
                                      ..^...^.....^..
                                      ...............
                                      .^.^.^.^.^...^.
                                      ...............
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(21, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(40, SmallInput);
    }

    [TestMethod]
    public void CalculatePathCache_WorksFor_Trivial()
    {
        const string input = """
                             .S.
                             ...
                             .^.
                             ...
                             """;

        var map = Problem07.Map.Parse(input);

        var pathCount = map.CalculatePathCache();

        pathCount.Should().Be(2);
    }

    [TestMethod]
    public void CalculatePathCache_WorksFor_Simple()
    {
        const string input = """
                             .S...
                             .....
                             .^...
                             .....
                             ..^..
                             .....
                             """;

        var map = Problem07.Map.Parse(input);

        var pathCount = map.CalculatePathCache();

        pathCount.Should().Be(3);
    }
}