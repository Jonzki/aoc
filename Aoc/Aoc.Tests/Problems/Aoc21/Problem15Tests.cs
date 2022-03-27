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
public class Problem15Tests : ProblemTests<Problem15>
{
    protected override string SmallInput => Problem15.SmallInput;

    protected override object CorrectOutput1 => 40L;

    protected override object CorrectOutput2 => 0L;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [TestMethod]
    public void Part1_MiniInput_Is_Correct()
    {
        string miniInput = @"19111
19191
11191";
        long correctOutput = 10L;

        RunPart1(correctOutput, miniInput);
    }

}
