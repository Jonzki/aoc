using Aoc.Problems.Aoc18;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem01Tests : ProblemTests<Problem01>
{
    [DataTestMethod]
    [DataRow("+1, -2, +3, +1", 3)]
    [DataRow("+1, +1, +1", 3)]
    [DataRow("+1, +1, -2", 0)]
    [DataRow("-1, -2, -3", -6)]
    public void Part1_SmallInput_Is_Correct(string input, int correctOutput) => RunPart1(correctOutput, input);

    [DataTestMethod]
    [DataRow("+1, -2, +3, +1", 2)]
    [DataRow("+1, -1", 0)]
    [DataRow("+3, +3, +4, -2, -4", 10)]
    [DataRow("-6, +3, +8, +5, -6", 5)]
    [DataRow("+7, +7, -2, -7, -4", 14)]
    public void Part2_SmallInput_Is_Correct(string input, int correctOutput) => RunPart2(correctOutput, input);
}
