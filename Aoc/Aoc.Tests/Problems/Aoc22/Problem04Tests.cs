using Aoc.Problems.Aoc22;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem04Tests : ProblemTests<Problem04>
{
    private string SmallInput => @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8";

    private object CorrectOutput1 => 2;

    private object CorrectOutput2 => 4;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
