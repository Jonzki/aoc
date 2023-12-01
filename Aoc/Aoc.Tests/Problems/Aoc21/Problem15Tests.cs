using Aoc.Problems.Aoc21;
using Aoc.Utils;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem15Tests : ProblemTests<Problem15>
{
    private string SmallInput => Problem15.SmallInput;

    private object CorrectOutput1 => 40;

    private object CorrectOutput2 => 315;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [DataTestMethod]
    [DataRow(2, Problem15.MiniInput)]
    [DataRow(4, Problem15.MiniInput2)]
    public void Part1_MiniInput_Is_Correct(int correctOutput, string input)
    {
        RunPart1(correctOutput, input);
    }

    [TestMethod]
    public void ParseMap2_Works()
    {
        string input = "8";

        var correctOutput = @"89123
91234
12345
23456
34567";
        var outputMap = ArrayUtils.To2D(correctOutput
            .SplitLines()
            .SelectMany(l => l.Select(c => c - '0'))
            .ToArray(), 5, 5);

        var (nodes, w, h) = Problem15.ParseMap2(input);

        Assert.AreEqual(25, nodes.Count);
        foreach (var node in nodes)
        {
            var correctValue = outputMap[node.Position.Y, node.Position.X];
            Assert.AreEqual(correctValue, node.Risk);
        }
    }

}
