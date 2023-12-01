using Aoc.Problems.Aoc21;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem10Tests : ProblemTests<Problem10>
{
    private string SmallInput => @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";

    private object CorrectOutput1 => 26397;

    private object CorrectOutput2 => 288957L;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [DataTestMethod]
    // Valid lines
    [DataRow("()", 0)]
    [DataRow("[]", 0)]
    [DataRow("([])", 0)]
    [DataRow("{()()()}", 0)]
    [DataRow("<([{}])>", 0)]
    [DataRow("[<>({}){}[([])<>]]", 0)]
    [DataRow("(((((((((())))))))))", 0)]
    // Incomplete line
    [DataRow("()(", 0)]
    // Corrupt line
    [DataRow("[)", 3)]
    [DataRow("(]", 57)]
    [DataRow("(}", 1197)]
    [DataRow("(>", 25137)]
    public void GetScore_Parses_Examples(string line, int score)
    {
        Assert.AreEqual(score, Problem10.GetScore(line));
    }
}
