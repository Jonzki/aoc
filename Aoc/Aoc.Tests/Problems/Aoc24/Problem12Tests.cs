using System.Linq;
using Aoc.Problems.Aoc24;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem12Tests : ProblemTests<Problem12>
{
    public const string SmallInput1 = """
                                      AAAA
                                      BBCD
                                      BBCC
                                      EEEC
                                      """;

    public const string SmallInput2 = """
                                      OOOOO
                                      OXOXO
                                      OOOOO
                                      OXOXO
                                      OOOOO
                                      """;

    public const string SmallInput3 = """
                                      RRRRIICCFF
                                      RRRRIICCCF
                                      VVRRRCCFFF
                                      VVRCCCJFFF
                                      VVVVCJJCFE
                                      VVIVCCJJEE
                                      VVIIICJJEE
                                      MIIIIIJJEE
                                      MIIISIJEEE
                                      MMMISSJEEE
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(140, SmallInput1);

        RunPart1(772, SmallInput2);

        RunPart1(1930, SmallInput3);
    }

    [TestMethod]
    public void SolvePart2()
    {
        // Nothing to test: Part 2 simply requests 75 passes instead of 25.
    }

    [TestMethod]
    public void ParseRegions_WorksWith_SmallInput1()
    {
        var map = Problem12.ParseMap(SmallInput1);

        var regions = Problem12.ParseRegions(map);

        regions.Should().HaveCount(5);

        // Check region sizes.
        regions.Should().Satisfy(
            r => r.Character == 'A' && r.Points.Count == 4,
            r => r.Character == 'B' && r.Points.Count == 4,
            r => r.Character == 'C' && r.Points.Count == 4,
            r => r.Character == 'D' && r.Points.Count == 1,
            r => r.Character == 'E' && r.Points.Count == 3
        );

        // Sanity check a couple of the positions.
        var regionB = regions.First(r => r.Character == 'B');
        regionB.Points.Should().Contain([
            new Point2D(0, 1),
            new Point2D(1, 1),
            new Point2D(0, 2),
            new Point2D(1, 2)
        ]);

        // Sanity check a couple of the positions.
        var regionC = regions.First(r => r.Character == 'C');
        regionC.Points.Should().Contain([
            new Point2D(2, 1),
            new Point2D(2, 2),
            new Point2D(3, 2),
            new Point2D(3, 3)
        ]);
    }

    [TestMethod]
    public void ParseRegions_WorksWith_Surrounded()
    {
        var input = """
                    AAA
                    ABA
                    AAA
                    """;
        var map = Problem12.ParseMap(input);

        var regions = Problem12.ParseRegions(map);

        regions.Should().HaveCount(2);

        regions.Should().Satisfy(
            r => r.Character == 'A' && r.Points.Count == 8,
            r => r.Character == 'B' && r.Points.Count == 1 && r.Points.First().PositionEquals(1, 1)
        );
    }

    [TestMethod]
    public void Perimeter_WorksWith_SmallInput1()
    {
        var regionA = new Problem12.Region
        {
            Character = 'A',
            Points = { new(0, 0), new(1, 0), new(2, 0), new(3, 0) }
        };
        regionA.Perimeter().Should().Be(10);

        var regionC = new Problem12.Region
        {
            Character = 'C',
            Points = { new(2, 1), new(2, 2), new(3, 2), new(3, 3) }
        };
        regionC.Perimeter().Should().Be(10);

        var regionD = new Problem12.Region
        {
            Character = 'C',
            Points = { new(0, 0) }
        };
        regionD.Perimeter().Should().Be(4);
    }

    [TestMethod]
    public void Perimeter_WorksWith_SmallInput2()
    {
        var map = Problem12.ParseMap(SmallInput2);

        var regions = Problem12.ParseRegions(map);
        regions.Should().HaveCount(5);

        regions.Should().Contain(
            r => r.Character == 'O' && r.Points.Count == 21
        );
        regions
            .Count(r => r.Character == 'X' && r.Points.Count == 1)
            .Should().Be(4);

        var regionO = regions.First(r => r.Character == 'O');
        regionO.Area().Should().Be(21);
        regionO.Perimeter().Should().Be(36);

        // Each region 'X' should have an area of 1 and perimeter of 4.
        regions.Should().Contain(
            r => r.Character == 'X' && r.Area() == 1 && r.Perimeter() == 4
        );
    }

    [TestMethod]
    public void Sides_Works()
    {
        var regionA = new Problem12.Region
        {
            Character = 'A',
            Points = { new(0, 0), new(1, 0), new(2, 0), new(3, 0) }
        };
        regionA.Sides().Should().Be(4);

        var regionC = new Problem12.Region
        {
            Character = 'C',
            Points = { new(2, 1), new(2, 2), new(3, 2), new(3, 3) }
        };
        regionC.Sides().Should().Be(8);
    }
}