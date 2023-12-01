using Aoc.Problems.Aoc21;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem5Tests
{

    private const string SmallInput = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";

    [DataTestMethod]
    [DataRow(5, SmallInput)]
    public void Part1_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var output = new Problem5().Solve1(input);
        Assert.AreEqual(correctOutput, output);
    }

    [DataTestMethod]
    [DataRow(12, SmallInput)]
    public void Part2_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var output = new Problem5().Solve2(input);
        Assert.AreEqual(correctOutput, output);
    }

    [DataTestMethod]
    [DataRow(2, "1,1;1,2")]
    [DataRow(4, "1,1;4,1")]
    [DataRow(1, "1,1;1,1")]
    [DataRow(2, "2,2;2,1")]
    public void LineConstructor_Works_Correctly(int correctPointCount, string input)
    {
        var n = input.Split(',', ';').Select(int.Parse).ToArray();

        // Build a line.
        var line = new Problem5.Line(new Aoc.Utils.Point2D(n[0], n[1]), new Aoc.Utils.Point2D(n[2], n[3]));

        // Check point count.
        Assert.AreEqual(correctPointCount, line.Points.Count);
    }
}
