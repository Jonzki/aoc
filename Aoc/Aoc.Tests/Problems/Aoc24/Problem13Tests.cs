using Aoc.Problems.Aoc24;
using Aoc.Utils;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem13Tests : ProblemTests<Problem13>
{
    public const string SmallInput = """
                                     Button A: X+94, Y+34
                                     Button B: X+22, Y+67
                                     Prize: X=8400, Y=5400

                                     Button A: X+26, Y+66
                                     Button B: X+67, Y+21
                                     Prize: X=12748, Y=12176

                                     Button A: X+17, Y+86
                                     Button B: X+84, Y+37
                                     Prize: X=7870, Y=6450

                                     Button A: X+69, Y+23
                                     Button B: X+27, Y+71
                                     Prize: X=18641, Y=10279
                                     """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(480, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        // Nothing to test: Part 2 has a MUCH higher limit and no sample result is provided.
    }

    [TestMethod]
    public void ParseMachine_Works()
    {
        var machines = Problem13.ParseMachines(SmallInput);

        machines.Should().HaveCount(4);

        var m = machines.First();
        m.Should().NotBeNull();
        m.ButtonA.Should().Match<Point2D>(p => p.PositionEquals(94, 34));
        m.ButtonB.Should().Match<Point2D>(p => p.PositionEquals(22, 67));
        m.Prize.Should().Match<Point2D>(p => p.PositionEquals(8400, 5400));
    }

    [TestMethod]
    public void MachinePlay_WorksFor_SmallInput()
    {
        var machines = Problem13.ParseMachines(SmallInput);

        // Machine 1: possible with a cost of 280.
        var result = machines[0].Play();
        result.PossibleToWin.Should().BeTrue();
        result.TokenCost.Should().Be(280);

        // Machines 2 and 4 should not be possible to win.
        machines[1].Play().PossibleToWin.Should().Be(false);
        machines[3].Play().PossibleToWin.Should().Be(false);

        // Machine 3 should be winnable with a cost of 200.
        result = machines[2].Play();
        result.PossibleToWin.Should().BeTrue();
        result.TokenCost.Should().Be(200);
    }
}