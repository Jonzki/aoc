using Aoc.Problems.Aoc22;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem02Tests : ProblemTests<Problem02>
{
    private string SmallInput => @"A Y
B X
C Z";

    private object CorrectOutput1 => 15;

    private object CorrectOutput2 => 12;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
