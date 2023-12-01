using Aoc.Problems.Aoc21;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem6Tests : ProblemTests<Problem6>
{
    private string SmallInput => @"3,4,3,1,2";

    private object CorrectOutput1 => 5934L;

    private object CorrectOutput2 => 26984457539L;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
