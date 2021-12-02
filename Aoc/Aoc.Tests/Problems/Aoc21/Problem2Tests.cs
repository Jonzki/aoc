using Aoc.Problems.Aoc21;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem2Tests
{
    [DataTestMethod]
    [DataRow(150, "forward 5;down 5;forward 8;up 3;down 8;forward 2")]
    public void Part1_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var array = input.Split(';');
        var coordinates = Problem2.Navigate1(array);
        Assert.AreEqual((int)correctOutput, coordinates.Position * coordinates.Depth);
    }

    [DataTestMethod]
    [DataRow(900, "forward 5;down 5;forward 8;up 3;down 8;forward 2")]
    public void Part2_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var array = input.Split(';');
        var coordinates = Problem2.Navigate2(array);
        Assert.AreEqual((int)correctOutput, coordinates.Position * coordinates.Depth);
    }
}
