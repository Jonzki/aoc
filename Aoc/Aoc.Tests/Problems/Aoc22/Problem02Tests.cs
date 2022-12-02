using Aoc.Problems.Aoc22;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem02Tests : ProblemTests<Problem02>
{
    protected override string SmallInput => @"A Y
B X
C Z";

    protected override object CorrectOutput1 => 15;

    protected override object CorrectOutput2 => 12;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
