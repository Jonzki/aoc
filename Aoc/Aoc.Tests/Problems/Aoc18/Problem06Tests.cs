using Aoc.Problems.Aoc18;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem06Tests : ProblemTests<Problem06>
{
    private const string SmallInput = @"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9";

    private object CorrectOutput1 => 17;

    private object CorrectOutput2 => 4;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
