using Aoc.Problems.Aoc24;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem11Tests : ProblemTests<Problem11>
{
    public const string SmallInput = "125 17";

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(55312, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        // Nothing to test: Part 2 simply requests 75 passes instead of 25.
    }

    [DataTestMethod]
    [DataRow(99, 9, 9)]
    [DataRow(1234, 12, 34)]
    [DataRow(100001, 100, 1)]
    [DataRow(1000, 10, 0)]
    public void Split_Works(long input, long left, long right)
    {
        var output = Problem11.Split(input);

        output.Left.Should().Be(left);
        output.Right.Should().Be(right);
    }

    [DataTestMethod]
    [DataRow(0, 1, -1)]
    [DataRow(1, 2024, -1)]
    [DataRow(10, 1, 0)]
    [DataRow(99, 9, 9)]
    [DataRow(999, 2021976, -1)]
    public void Blink_Works(long stone, long left, long right)
    {
        var output = Problem11.Blink(stone);

        output.Left.Should().Be(left);
        output.Right.Should().Be(right);
    }
}