using Aoc.Problems.Aoc21;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem4Tests
{

    private const string SmallInput = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";

    [TestMethod]
    public void ParseInput_Handles_SmallInput()
    {
        var (boards, numbers) = Problem4.ParseInput(SmallInput);

        Assert.AreEqual(27, numbers.Count);
        Assert.AreEqual(3, boards.Count);
    }

    [DataTestMethod]
    [DataRow(22, 13, 17, 11, 0)]
    [DataRow(13, 2, 9, 10, 12)]
    public void CheckBingo_Checks_Bingo(params int[] numbers)
    {
        var (boards, _) = Problem4.ParseInput(SmallInput);
        var board = boards[0];

        var isBingo = false;
        foreach (var num in numbers)
        {
            isBingo |= board.AddNumber(num);
        }
        Assert.IsTrue(isBingo);
    }

    [DataTestMethod]
    [DataRow(4512, SmallInput)]
    public void Part1_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var output = new Problem4().Solve1(input);
        Assert.AreEqual(correctOutput, output);
    }

    [DataTestMethod]
    [DataRow(1924, SmallInput)]
    public void Part2_SmallInput_Is_Correct(object correctOutput, string input)
    {
        var output = new Problem4().Solve2(input);
        Assert.AreEqual(correctOutput, output);
    }
}
