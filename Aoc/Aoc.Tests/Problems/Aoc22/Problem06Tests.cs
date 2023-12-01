using Aoc.Problems.Aoc22;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem06Tests : ProblemTests<Problem06>
{
    [DataTestMethod]
    [DataRow("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
    [DataRow("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
    [DataRow("nppdvjthqldpwncqszvftbrmjlhg", 6)]
    [DataRow("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
    [DataRow("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
    public void Part1_SmallInput_Is_Correct(string input, int correctOutput) => RunPart1(correctOutput, input);

    [DataTestMethod]
    [DataRow("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
    [DataRow("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
    [DataRow("nppdvjthqldpwncqszvftbrmjlhg", 23)]
    [DataRow("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
    [DataRow("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
    public void Part2_SmallInput_Is_Correct(string input, int correctOutput) => RunPart2(correctOutput, input);
}
