using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem3 : IProblem
{
    public object Solve1(string input)
    {
        var binaries = ProblemInput.ParseList(input);
        return MeasurePower(binaries);
    }

    public object Solve2(string input)
    {
        var binaries = ProblemInput.ParseList(input);
        return LifeSupportRating(binaries);
    }

    /// <summary>
    /// Day 3 part 1: measure power consumption based on first and last bits of the input binary data.
    /// </summary>
    /// <param name="binaryList"></param>
    /// <returns></returns>
    public static int MeasurePower(string[] binaryList)
    {
        if (!binaryList.Any()) return 0;

        var binaries = binaryList.Select(BitArrayUtils.Parse).ToArray();

        // Calculate the occurrences of each bit.
        var binaryLength = binaries.First().Length;

        // Convert into gamma and epsilon binaries (most/least common bit).
        var gammaBinary = new BitArray(binaryLength);
        var epsilonBinary = new BitArray(binaryLength);

        for (var i = 0; i < binaryLength; i++)
        {
            gammaBinary[i] = MostCommonBit(binaries, i);
            epsilonBinary[i] = !gammaBinary[i];
        }

        var gamma = gammaBinary.ToInteger();
        var epsilon = epsilonBinary.ToInteger();

        return gamma * epsilon;
    }

    public static int LifeSupportRating(string[] binaryList)
    {
        if (binaryList.Length == 0) return 0;

        var remainingForOxygen = binaryList.Select(BitArrayUtils.Parse).ToList();
        var remainingForScrubber = binaryList.Select(BitArrayUtils.Parse).ToList();

        var binaryLength = binaryList.First().Length;

        for (var i = 0; i < binaryLength && remainingForOxygen.Count > 1; ++i)
        {
            // Find the most common bit in the position.
            var mostCommonOxygen = MostCommonBit(remainingForOxygen, i);

            // Filter out binaries that do not match.
            remainingForOxygen = remainingForOxygen.Where(b => b[i] == mostCommonOxygen).ToList();
        }
        for (var i = 0; i < binaryLength && remainingForScrubber.Count > 1; ++i)
        {
            // Find the most common bit in the position.
            var leastCommonScrubber = !MostCommonBit(remainingForScrubber, i);

            remainingForScrubber = remainingForScrubber.Where(b => b[i] == leastCommonScrubber).ToList();
        }

        return remainingForOxygen[0].ToInteger() * remainingForScrubber[0].ToInteger();
    }

    /// <summary>
    /// Finds the most common bit at the given position from the input binary list.
    /// </summary>
    /// <param name="binaries"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static bool MostCommonBit(IEnumerable<BitArray> binaries, int position)
    {
        var ones = 0;
        foreach (var binary in binaries)
        {
            if (binary[position]) ones++;
        }
        return ones >= binaries.Count() / 2m;
    }

}
