using System.Formats.Asn1;
using System.Linq;
using Aoc.Problems.Aoc25;

namespace Aoc.Tests.Problems.Aoc25;

[TestClass]
public class Problem08Tests : ProblemTests<Problem08>
{
    private const string SmallInput = """
                                      162,817,812
                                      57,618,57
                                      906,360,560
                                      592,479,940
                                      352,342,300
                                      466,668,158
                                      542,29,236
                                      431,825,988
                                      739,650,466
                                      52,470,668
                                      216,146,977
                                      819,987,18
                                      117,168,530
                                      805,96,715
                                      346,949,466
                                      970,615,88
                                      941,993,340
                                      862,61,35
                                      984,92,344
                                      425,690,689
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        // RunPart1(40, SmallInput);

        // In part 1 sample, we run 10 iterations and then count the largest circuits.
        var map = Problem08.ParseMap(SmallInput);
        map.CalculatePointPairs();

        Problem08.ApplyConnections(map, 10);

        // Calculate the circuit sizes.
        var sizes = map.GetCircuitSizes();

        // After making the ten shortest connections, there are 11 circuits:
        sizes.Should().HaveCount(11);

        int[] correctSizes = [

            5, // one circuit which contains 5 junction boxes
            4, // one circuit which contains 4 junction boxes
            2, // two circuits which contain 2 junction boxes each
            2,
            1, // and seven circuits which each contain a single junction box
            1,
            1,
            1,
            1,
            1,
            1
        ];

        sizes.Should().BeEquivalentTo(correctSizes);

        // Correct output would be the multiplication of the first three.
        var result = sizes[0] * sizes[1] * sizes[2];
        result.Should().Be(40);
    }

    [TestMethod]
    public void SolvePart1_Detailed()
    {
        // RunPart1(40, SmallInput);

        // In part 1 sample, we run 10 iterations and then count the largest circuits.
        var map = Problem08.ParseMap(SmallInput);
        map.CalculatePointPairs();

        int pairIndex = 0;

        // Apply a single connection.
        Problem08.ApplySingleConnection(map, pairIndex++);

        // This should have connected points 162,817,812 and 425,690,689
        var pointA = map.Points.First(p => p.ToString() == "162,817,812");
        var pointB = map.Points.First(p => p.ToString() == "425,690,689");

        // Both should have the same non-zero circuit ID.
        pointA.CircuitId.Should().Be(pointB.CircuitId).And.BeGreaterThan(0);

        // Now, the two junction boxes which are closest together but aren't already directly connected are 162,817,812 and 431,825,988.
        Problem08.ApplySingleConnection(map, pairIndex++);

        // Find the new point C. This should have the same circuit ID as the two previous ones.
        var pointC = map.Points.First(p => p.ToString() == "431,825,988");

        pointC.CircuitId.Should().BeGreaterThan(0);
        new[] { pointA.CircuitId, pointB.CircuitId, pointC.CircuitId }.Distinct().Should().HaveCount(1);

        // The next two junction boxes to connect are 906,360,560 and 805,96,715.
        Problem08.ApplySingleConnection(map, pairIndex++);

        pointA = map.Points.First(p => p.ToString() == "906,360,560");
        pointB = map.Points.First(p => p.ToString() == "805,96,715");

        // Both should have the same non-zero circuit ID.
        pointA.CircuitId.Should().Be(pointB.CircuitId).And.BeGreaterThan(0);

        // After connecting them, there is a circuit containing 3 junction boxes, a circuit containing 2 junction boxes, and 15 circuits which contain one junction box each.
        var circuits = map.GetCircuitSizes();

        circuits.Should().ContainSingle(x => x == 3);
        circuits.Should().ContainSingle(x => x == 2);

        circuits.Where(x => x == 1).Should().HaveCount(15);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(25272, SmallInput);
    }

    [TestMethod]
    public void CalculatePointPairs_Works()
    {
        var map = Problem08.ParseMap(SmallInput);
        map.Points.Should().HaveCount(20);

        map.CalculatePointPairs();

        map.PointPairsByDistance.Count.Should().BeGreaterThanOrEqualTo(2);

        var firstPair = map.PointPairsByDistance[0];
        firstPair.PointA.Should().NotBe(firstPair.PointB);

        var pointA = map.Points[firstPair.PointA];
        var pointB = map.Points[firstPair.PointB];

        // From task info, closest pair in the small input should be 162,817,812 and 425,690,689
        pointA.ToString().Should().BeOneOf("162,817,812", "425,690,689");
        pointB.ToString().Should().BeOneOf("162,817,812", "425,690,689");

        var secondPair = map.PointPairsByDistance[1];
        secondPair.PointA.Should().NotBe(secondPair.PointB);

        pointA = map.Points[secondPair.PointA];
        pointB = map.Points[secondPair.PointB];

        // Next closest would be 162,817,812 and 431,825,988
        pointA.ToString().Should().BeOneOf("162,817,812", "431,825,988");
        pointB.ToString().Should().BeOneOf("162,817,812", "431,825,988");
    }
}