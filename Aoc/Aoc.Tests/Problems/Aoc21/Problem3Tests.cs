using Aoc.Problems.Aoc21;
using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem3Tests
{
    [DataTestMethod]
    [DataRow(198, "00100;11110;10110;10111;10101;01111;00111;11100;10000;11001;00010;01010")]
    public void Part1_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var binaryList = ProblemInput.ParseList(input);

        var output = Problem3.MeasurePower(binaryList);

        Assert.AreEqual(correctOutput, output);
    }

    [DataTestMethod]
    [DataRow(230, "00100;11110;10110;10111;10101;01111;00111;11100;10000;11001;00010;01010")]
    public void Part2_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var binaryList = ProblemInput.ParseList(input);

        var output = Problem3.LifeSupportRating(binaryList);

        Assert.AreEqual(correctOutput, output);
    }
}
