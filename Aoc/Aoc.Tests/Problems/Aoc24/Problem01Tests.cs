using Aoc.Problems.Aoc24;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem01Tests : ProblemTests<Problem01>
{
    private const string SmallInput = """
                                      3   4
                                      4   3
                                      2   5
                                      1   3
                                      3   9
                                      3   3
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1<int>(11, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(31L, SmallInput);
    }
}