using Aoc.Problems.Aoc25;
using static Aoc.Problems.Aoc25.Problem01;

namespace Aoc.Tests.Problems.Aoc25;

[TestClass]
public class Problem01Tests : ProblemTests<Problem01>
{
    [TestMethod]
    public void SolvePart1()
    {
        const string smallInput = """
                                  L68
                                  L30
                                  R48
                                  L5
                                  R60
                                  L55
                                  L1
                                  L99
                                  R14
                                  L82
                                  """;

        RunPart1(3, smallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        const string smallInput = """
                                  L68
                                  L30
                                  R48
                                  L5
                                  R60
                                  L55
                                  L1
                                  L99
                                  R14
                                  L82
                                  """;

        RunPart2(6, smallInput);
    }

    [TestMethod]
    public void DialRotate_Handles_Negative()
    {
        var dial = new Problem01.Dial(10);

        var clicks = dial.Rotate(-20);

        dial.Position.Should().Be(90);
        clicks.Should().Be(1);
    }

    [TestMethod]
    public void DialRotate_Handles_Positive()
    {
        var dial = new Problem01.Dial(90);

        var clicks = dial.Rotate(20);

        dial.Position.Should().Be(10);
        clicks.Should().Be(1);
    }

    [TestMethod]
    public void DialRotate_Handles_MultiRotate()
    {
        var dial = new Problem01.Dial(50);

        var clicks = dial.Rotate(1000);

        dial.Position.Should().Be(50);
        clicks.Should().Be(10);

        clicks = dial.Rotate(-1000);

        dial.Position.Should().Be(50);
        clicks.Should().Be(10);
    }

    [TestMethod]
    public void DialRotate_Handles_FromZero()
    {
        var dial = new Problem01.Dial(0);

        // Rotate off from zero.
        var clicks = dial.Rotate(10);
        dial.Position.Should().Be(10);
        clicks.Should().Be(0, "no click if started from zero");

        // Rotate back to zero.
        clicks = dial.Rotate(-10);
        dial.Position.Should().Be(0);
        clicks.Should().Be(1, "click because we reached zero");

        // Rotate back to zero.
        clicks = dial.Rotate(-10);
        dial.Position.Should().Be(90);
        clicks.Should().Be(0, "started from zero, did not roll over");

        // Rotate back to zero.
        clicks = dial.Rotate(-100);
        dial.Position.Should().Be(90);
        clicks.Should().Be(1, "started from 90, rolled over (left)");

        // Full rotation from zero.
        clicks = dial.Rotate(10);
        dial.Position.Should().Be(0);
        clicks.Should().Be(1, "click because we arrived back at zero from left");

        // Full rotation from zero.
        clicks = dial.Rotate(100);
        dial.Position.Should().Be(0);
        clicks.Should().Be(1, "click because we arrived back at zero");

        // Full rotation from zero.
        clicks = dial.Rotate(-200);
        dial.Position.Should().Be(0);
        clicks.Should().Be(2, "click because we made two full rotations");
    }
}