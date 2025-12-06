using Aoc.Problems.Aoc25;
using System.Collections.Generic;

namespace Aoc.Tests.Problems.Aoc25;

[TestClass]
public class Problem05Tests : ProblemTests<Problem05>
{
    private const string SmallInput = """
                                      3-5
                                      10-14
                                      16-20
                                      12-18

                                      1
                                      5
                                      8
                                      11
                                      17
                                      32
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(3, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(14, SmallInput);
    }

    [TestMethod]
    public void ParseInput_Works()
    {
        var parsed = Problem05.ParseInput(SmallInput);

        parsed.Ranges.Should().NotBeNull().And.HaveCount(4);
        parsed.Ingredients.Should().NotBeNull().And.HaveCount(6);

        parsed.Ranges.Should().Contain(x => x.Start == 16L && x.End == 20L);
        parsed.Ingredients.Should().Contain(17L);
    }
}