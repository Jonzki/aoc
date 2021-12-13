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
public class Problem12Tests : ProblemTests<Problem12>
{
    protected override string SmallInput => @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

    protected override object CorrectOutput1 => 10;

    protected override object CorrectOutput2 => 36;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [TestMethod]
    public void Part2_MiniInput_Is_Correct()
    {
        // Allowed paths should be:
        // start-A-end
        // start-A-b-A-end
        // start-A-b-A-b-end
        string miniInput = @"start-A
end-A
A-b";

        RunPart2(3, miniInput);
    }
}
