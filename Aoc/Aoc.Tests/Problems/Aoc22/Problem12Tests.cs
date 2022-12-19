using Aoc.Problems.Aoc22;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem12Tests : ProblemTests<Problem12>
{
    const string SmallInput = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";

    const int CorrectOutput1 = 31;

    const int CorrectOutput2 = 29;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
