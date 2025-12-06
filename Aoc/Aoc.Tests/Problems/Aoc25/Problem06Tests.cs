using System;
using Aoc.Problems.Aoc25;

namespace Aoc.Tests.Problems.Aoc25;

[TestClass]
public class Problem06Tests : ProblemTests<Problem06>
{
    // Slightly different SmallInput building to make absolutely sure we include the spaces correctly.
    private static string SmallInput => string.Join(Environment.NewLine, [
        "123 328  51 64 ",
        " 45 64  387 23 ",
        "  6 98  215 314",
        "*   +   *   +  "
    ]);

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(4277556, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(3263827, SmallInput);
    }

    [TestMethod]
    public void ParseProblems1_WorksFor_SmallInput()
    {
        var problems = Problem06.ParseProblems1(SmallInput);

        problems.Should().HaveCount(4);
        problems.Should().Contain(x =>
            x.Operand == '+'
            && x.Numbers.Contains(328)
            && x.Numbers.Contains(64)
            && x.Numbers.Contains(98));
    }

    [TestMethod]
    public void ParseProblems2_WorksFor_SmallInput()
    {
        var problems = Problem06.ParseProblems2(SmallInput);

        problems.Should().HaveCount(4);
        problems.Should().Contain(x =>
            x.Operand == '+'
            && x.Numbers.Contains(8)
            && x.Numbers.Contains(248)
            && x.Numbers.Contains(369));

        problems.Should().Contain(x =>
            x.Operand == '*'
            && x.Numbers.Contains(356)
            && x.Numbers.Contains(24)
            && x.Numbers.Contains(1));
    }
}