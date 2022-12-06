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
public class Problem11Tests : ProblemTests<Problem11>
{
    private string SmallInput => @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

    private object CorrectOutput1 => 1656L;

    private object CorrectOutput2 => 195;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [DataTestMethod]
    [DataRow(3, "111111111", "222222222")]
    [DataRow(3, "111191111", "333303333")]
    public void Simulate_Handles_Cases(int w, string input, string output)
    {
        var inputArray = input.Select(c => c - '0').ToArray();
        var outputArray = output.Select(c => c - '0').ToArray();

        // Run a single simulation on the input.
        Problem11.Simulate(inputArray, w, w);

        // Input should now match the output.
        Console.WriteLine("IN  - OUT");
        for (var y = 0; y < w; ++y)
        {
            for (var x = 0; x < w; ++x)
            {
                Console.Write(inputArray[ArrayUtils.GetIndex(x, y, w)]);
            }
            Console.Write(" - ");
            for (var x = 0; x < w; ++x)
            {
                Console.Write(outputArray[ArrayUtils.GetIndex(x, y, w)]);
            }
            Console.WriteLine("");
        }

        CollectionAssert.AreEqual(outputArray, inputArray);
    }



}
