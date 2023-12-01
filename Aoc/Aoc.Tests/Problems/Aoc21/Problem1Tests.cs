using Aoc.Problems.Aoc21;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem1Tests
{
    [DataTestMethod]
    [DataRow(7, 199, 200, 208, 210, 200, 207, 240, 269, 260, 263)]
    public void Part1_SmallInput_Is_Correct(object correctOutput, params object[] numbers)
    {
        var array = numbers.Select(x => (int)x).ToArray();
        Assert.AreEqual((int)correctOutput, Problem1.FindIncrements(array));
    }

    [DataTestMethod]
    [DataRow(5, 199, 200, 208, 210, 200, 207, 240, 269, 260, 263)]
    public void Part2_SmallInput_Is_Correct(object correctOutput, params object[] numbers)
    {
        var array = numbers.Select(x => (int)x).ToArray();
        Assert.AreEqual((int)correctOutput, Problem1.FindWindowIncrements(array));
    }
}
