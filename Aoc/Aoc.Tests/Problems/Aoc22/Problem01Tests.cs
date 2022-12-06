using Aoc.Problems.Aoc22;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem01Tests : ProblemTests<Problem01>
{
    private string SmallInput => @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";

    private object CorrectOutput1 => 24000;

    private object CorrectOutput2 => 45000;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
