using Aoc.Problems.Aoc23;

namespace Aoc.Tests.Problems.Aoc23;

[TestClass]
public class Problem01Tests : ProblemTests<Problem01>
{
    private const string SmallInput1 = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";

    private const string SmallInput2 = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen";

    [TestMethod]
    public void Solve1Test()
    {
        RunPart1(142, SmallInput1);
    }

    [TestMethod]
    public void Solve2Test()
    {
        RunPart2(281, SmallInput2);
    }

    [DataTestMethod]
    [DataRow("1abc2", 12)]
    [DataRow("2", 22)]
    [DataRow("abc", 0)]
    [DataRow("123456", 16)]
    public void LineValue1_Works(string line, int correctValue)
    {
        var value = Problem01.LineValue1(line);
        Assert.AreEqual(correctValue, value);
    }

    [DataTestMethod]
    [DataRow("1abc2", 12)]
    [DataRow("two1nine", 29)]
    [DataRow("eightwothree", 83)]
    [DataRow("abcone2threexyz", 13)]
    [DataRow("xtwone3four", 24)]
    [DataRow("4nineeightseven2", 42)]
    [DataRow("zoneight234", 14)]
    [DataRow("7pqrstsixteen", 76)]
    public void LineValue2_Works(string line, int correctValue)
    {
        var value = Problem01.LineValue2(line);
        Assert.AreEqual(correctValue, value);
    }
}