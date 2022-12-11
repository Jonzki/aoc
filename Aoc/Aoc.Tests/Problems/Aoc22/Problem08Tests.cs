using Aoc.Problems.Aoc22;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem08Tests : ProblemTests<Problem08>
{
    const string SmallInput = @"30373
25512
65332
33549
35390";

    const int CorrectOutput1 = 21;

    const int CorrectOutput2 = 8;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
