using Aoc.Problems.Aoc19;
using System;

namespace Aoc.Tests.Problems.Aoc19;

[TestClass]
public class Problem1Tests
{
    [DataTestMethod]
    [DataRow(12, 2)]
    [DataRow(14, 2)]
    [DataRow(1969, 654)]
    [DataRow(100756, 33583)]
    public void Part1_SmallInput_Is_Correct(object input, object correct)
    {
        Assert.AreEqual(Convert.ToInt64(correct), Problem1.CalculateFuel1(Convert.ToInt64(input)));
    }

    [DataTestMethod]
    [DataRow(14, 2)]
    [DataRow(1969, 966)]
    [DataRow(100756, 50346)]
    public void Part2_SmallInput_Is_Correct(object input, object correct)
    {
        Assert.AreEqual(Convert.ToInt64(correct), Problem1.CalculateFuel2(Convert.ToInt64(input)));
    }
}