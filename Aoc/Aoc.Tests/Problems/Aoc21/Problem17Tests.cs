using Aoc.Problems.Aoc21;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem17Tests : ProblemTests<Problem17>
{
    protected override string SmallInput => "20,30,-10,-5";

    protected override object CorrectOutput1 => 45;

    protected override object CorrectOutput2 => 0L;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct()
        => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct()
        => RunPart2(CorrectOutput2, SmallInput);

}
