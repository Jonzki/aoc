using Aoc.Problems.Aoc23;

namespace Aoc.Tests.Problems.Aoc23;

[TestClass]
public class Problem05Tests : ProblemTests<Problem05>
{
    private const string SmallInput = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";

    [TestMethod]
    public void Part1_WorksWith_SmallInput()
    {
        RunPart1<long>(35, SmallInput);
    }

    [TestMethod]
    public void Part2_WorksWith_SmallInput()
    {
        RunPart2<long>(46, SmallInput);
    }

    [TestMethod]
    public void ParseInput_Works()
    {
        var (seeds, maps) = Problem05.ParseInput(SmallInput);

        Assert.AreEqual(4, seeds.Length);
        Assert.AreEqual(7, maps.Length);

        CollectionAssert.AreEqual(seeds, new[] { 79L, 14L, 55L, 13L });

        var firstMap = maps[0];
        CollectionAssert.AreEqual(firstMap.DestinationRangeStart, new long[] { 50, 52 });
        CollectionAssert.AreEqual(firstMap.SourceRangeStart, new long[] { 98, 50 });
        CollectionAssert.AreEqual(firstMap.RangeLength, new long[] { 2, 48 });
    }

    [TestMethod]
    public void MapNumber_Works()
    {
        var input = @"seeds: 79

seed-to-soil map:
50 98 2
52 50 48";

        var (seeds, maps) = Problem05.ParseInput(input);
        var map = maps[0];

        var output = map.MapNumber(seeds[0]);

        Assert.AreEqual(81L, output);
    }
}