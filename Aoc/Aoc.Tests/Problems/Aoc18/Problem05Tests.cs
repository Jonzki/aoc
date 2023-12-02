using System;
using Aoc.Problems.Aoc18;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem05Tests : ProblemTests<Problem05>
{
    private const string SmallInput = "dabAcCaCBAcCcaDA";

    private object CorrectOutput1 => 10;

    private object CorrectOutput2 => 4;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}