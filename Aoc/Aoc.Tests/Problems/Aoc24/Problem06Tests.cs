using Aoc.Problems.Aoc24;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem06Tests : ProblemTests<Problem06>
{
    private const string SmallInput = """
                                      ....#.....
                                      .........#
                                      ..........
                                      ..#.......
                                      .......#..
                                      ..........
                                      .#..^.....
                                      ........#.
                                      #.........
                                      ......#...
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(41, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(6, SmallInput);
    }
}