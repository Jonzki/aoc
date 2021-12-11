using Aoc.Problems.Aoc21;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem7Tests : ProblemTests<Problem7>
{
    protected override string SmallInput => @"16,1,2,0,4,2,7,1,2,14";

    protected override object CorrectOutput1 => 37;

    protected override object CorrectOutput2 => 168;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
