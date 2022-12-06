using Aoc.Problems.Aoc21;
using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem13Tests : ProblemTests<Problem13>
{
    private string SmallInput => @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

    private object CorrectOutput1 => 17;

    private object CorrectOutput2 => 36;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    // Part2 prints a text to the output console - not feasible to test.
    //[TestMethod]
    //public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
