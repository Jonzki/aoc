using Aoc.Problems.Aoc18;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem05Tests : ProblemTests<Problem05>
{
    private const string SmallInput = @"dabAcCaCBAcCcaDA";

    private object CorrectOutput1 => 10;

    private object CorrectOutput2 => 4;

    [DataTestMethod]
    [DataRow("aA", 0)]
    [DataRow("abBA", 0)]
    [DataRow(SmallInput, 10)]
    public void Part1_SmallInput_Is_Correct(string input, int correctOutput) => RunPart1(correctOutput, input);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
