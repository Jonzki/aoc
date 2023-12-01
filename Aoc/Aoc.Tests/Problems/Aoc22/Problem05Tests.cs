using Aoc.Problems.Aoc22;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem05Tests : ProblemTests<Problem05>
{
    private string SmallInput => @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    private object CorrectOutput1 => "CMZ";

    private object CorrectOutput2 => "MCD";

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
