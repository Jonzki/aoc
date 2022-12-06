using Aoc.Problems.Aoc21;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem9Tests : ProblemTests<Problem9>
{
    private string SmallInput => @"2199943210
3987894921
9856789892
8767896789
9899965678";

    private object CorrectOutput1 => 15;

    private object CorrectOutput2 => 1134;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
