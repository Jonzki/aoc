using Aoc.Problems.Aoc21;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem17Tests : ProblemTests<Problem17>
{
    private string SmallInput => "20,30,-10,-5";

    private object CorrectOutput1 => 45;

    private object CorrectOutput2 => 112;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct()
        => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct()
        => RunPart2(CorrectOutput2, SmallInput);

}
