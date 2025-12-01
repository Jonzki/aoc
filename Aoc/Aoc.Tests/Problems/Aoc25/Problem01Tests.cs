using Aoc.Problems.Aoc25;

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
        Assert.Inconclusive("TODO");
    }
}