using Aoc.Problems.Aoc18;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem02Tests : ProblemTests<Problem02>
{
    const string SmallInput1 = @"abcdef
bababc
abbcde
abcccd
aabcdd
abcdee
ababab";

    const string SmallInput2 = @"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz";

    private object CorrectOutput1 => 12;

    private object CorrectOutput2 => "fgij";

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput1);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput2);
}
