using System.Collections.Generic;
using Aoc.Problems.Aoc24;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem08Tests : ProblemTests<Problem08>
{
    public const string SmallInput = """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(14, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(34, SmallInput);
    }

    [TestMethod]
    public void BuildPairs_Builds_Pairs()
    {
        var mapPoints = new List<Problem08.MapPoint>()
        {
            // 2 "a" items form a pair 1-2
            new() { Char = 'a', Id = 1, Position = (1,1)},
            new() { Char = 'a', Id = 2, Position = (2,2)},
            // 3 "b" items form 3 possible pairs: 3-4, 3-5 and 4-5
            new() { Char = 'b', Id = 3, Position = (3,3)},
            new() { Char = 'b', Id = 4, Position = (4,4)},
            new() { Char = 'b', Id = 5, Position = (5,5)},
        };

        var pairs = Problem08.BuildPairs(mapPoints);

        // Should get 4 pairs (1 for a, 3 for b).
        pairs.Should().HaveCount(1 + 3);

        // "a" pair
        pairs.Should().Contain(p => Pair(p, 1, 2));

        // "b" pairs
        pairs.Should()
            .Contain(p => Pair(p, 3, 4))
            .And.Contain(p => Pair(p, 3, 5))
            .And.Contain(p => Pair(p, 4, 5));
    }

    private bool Pair((int A, int B) pair, int a, int b)
    {
        return pair.A == a && pair.B == b
            || pair.B == a && pair.A == b;
    }
}