using Aoc.Problems.Aoc22;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem09Tests : ProblemTests<Problem09>
{
    const string SmallInput = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

    const string SmallInput2 = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";

    const int CorrectOutput1 = 13;

    const int CorrectOutput2 = 1;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [TestMethod]
    public void Part2_SmallInput2_Is_Correct()
    {
        RunPart2(36, SmallInput2);
    }
}
