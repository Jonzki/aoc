using Aoc.Problems.Aoc24;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem02Tests : ProblemTests<Problem02>
{
    private const string SmallInput = """
                                      7 6 4 2 1
                                      1 2 7 8 9
                                      9 7 6 2 1
                                      1 3 2 4 5
                                      8 6 4 4 1
                                      1 3 6 7 9
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(2, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(4, SmallInput);
    }

    [DataTestMethod]
    [DataRow("7 6 4 2 1", true)]
    [DataRow("1 2 7 8 9", false)]
    [DataRow("9 7 6 2 1", false)]
    [DataRow("1 3 2 4 5", false)]
    [DataRow("8 6 4 4 1", false)]
    [DataRow("1 3 6 7 9", true)]
    public void IsSafe_WorksFor_SmallInputRows(string input, bool correctIsSafe)
    {
        // Parse input into numeric array.
        var report = input.Split(' ').Select(int.Parse).ToArray();

        // Then test the IsSafe method.
        Problem02.IsSafe1(report).Should().Be(correctIsSafe);
    }

    [DataTestMethod]
    [DataRow("7 6 4 2 1", true)]
    [DataRow("1 2 7 8 9", false)]
    [DataRow("9 7 6 2 1", false)]
    [DataRow("1 3 2 4 5", true)]
    [DataRow("8 6 4 4 1", true)]
    [DataRow("1 3 6 7 9", true)]
    public void IsSafe_WorksFor_SmallInputRows_Part2(string input, bool correctIsSafe)
    {
        // Parse input into numeric array.
        var report = input.Split(' ').Select(int.Parse).ToArray();

        // Then test the IsSafe method.
        Problem02.IsSafe2(report).Should().Be(correctIsSafe);
    }
}