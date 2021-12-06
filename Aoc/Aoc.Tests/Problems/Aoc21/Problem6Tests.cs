using Aoc.Problems.Aoc21;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem6Tests : ProblemTests<Problem6>
{
    protected override string SmallInput => @"3,4,3,1,2";

    protected override object CorrectOutput1 => 5934L;

    protected override object CorrectOutput2 => 26984457539L;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
