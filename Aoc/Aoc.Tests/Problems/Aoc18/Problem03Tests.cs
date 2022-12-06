using Aoc.Problems.Aoc18;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem03Tests : ProblemTests<Problem03>
{
    private const string SmallInput = @"#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2";

    private object CorrectOutput1 => 4;

    private object CorrectOutput2 => 3;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
