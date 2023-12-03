using Aoc.Problems.Aoc23;

namespace Aoc.Tests.Problems.Aoc23;

[TestClass]
public class Problem03Tests : ProblemTests<Problem03>
{
    private const string SmallInput = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

    [TestMethod]
    public void Part1_WorksWith_SmallInput()
    {
        RunPart1(4361, SmallInput);
    }

    [TestMethod]
    public void Part2_WorksWith_SmallInput()
    {
        RunPart2(467835, SmallInput);
    }
}